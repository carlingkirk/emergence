using System;

namespace Emergence.Transform.USDA
{
    public static class ChecklistParser
    {
        public static (string Genus, string Species, string Author, string Subspecies, string Variety, string Subvariety)
            ParseScientificNameWithAuthor(string scientificNameWithAuthor)
        {
            var scientificNameParts = scientificNameWithAuthor.Split(" ");

            var hasSpecies = true;
            if (char.IsUpper(scientificNameParts[1][0]) || scientificNameParts[1][0] == '(')
            {
                hasSpecies = false;
            }

            if (scientificNameParts.Length >= 3)
            {
                // It has genus, species and multiple author words
                var author = "";
                var variety = "";
                var subvariety = "";
                var subspecies = "";


                // Is it a var. or ssp.? If so we don't care about the first author
                var isVariety = scientificNameWithAuthor.Contains(" var. ");
                var isSubspecies = scientificNameWithAuthor.Contains(" ssp. ");
                var isSubvariety = scientificNameWithAuthor.Contains(" subvar. ");

                if (isVariety || isSubspecies || isSubvariety)
                {
                    byte? found = null;
                    for (byte i = 0; i < scientificNameParts.Length; i++)
                    {
                        if (found != null)
                        {
                            if (i == found + 1)
                            {
                                if (isVariety)
                                {
                                    variety = scientificNameParts[i];
                                }
                                else if (isSubspecies)
                                {
                                    subspecies = scientificNameParts[i];
                                }
                                else if (isSubvariety)
                                {
                                    subvariety = scientificNameParts[i];
                                }
                            }
                            else
                            {
                                author += scientificNameParts[i] + " ";
                            }
                        }
                        else if ((isVariety && scientificNameParts[i] == "var.") ||
                                 (isSubspecies && scientificNameParts[i] == "ssp.") ||
                                 (isSubvariety && scientificNameParts[i] == "subvar."))
                        {
                            // it's a ssp or subvar
                            if (!isVariety)
                            {
                                found = i;
                            }
                            // it's a ssp and a var
                            else if (isSubspecies && string.IsNullOrEmpty(subspecies))
                            {
                                subspecies = scientificNameParts[i + 1];
                            }
                            // it's a var
                            else
                            {
                                found = i;
                            }
                        }
                    }

                    if (!hasSpecies)
                    {
                        return (scientificNameParts[0], null, author.Trim(),
                            string.IsNullOrEmpty(subspecies) ? null : subspecies,
                            string.IsNullOrEmpty(variety) ? null : variety,
                            string.IsNullOrEmpty(subvariety) ? null : subvariety);
                    }
                    else
                    {
                        return (scientificNameParts[0], scientificNameParts[1], author.Trim(),
                            string.IsNullOrEmpty(subspecies) ? null : subspecies,
                            string.IsNullOrEmpty(variety) ? null : variety,
                            string.IsNullOrEmpty(subvariety) ? null : subvariety);
                    }

                }
                else
                {
                    var start = hasSpecies ? 2 : 1;

                    for (var i = start; i < scientificNameParts.Length; i++)
                    {
                        author += scientificNameParts[i] + " ";
                    }

                    var species = hasSpecies ? scientificNameParts[1] : null;

                    return (scientificNameParts[0], species, author.Trim(), null, null, null);
                }
            }
            else if (scientificNameParts.Length == 2)
            {
                // Is the second word an author?
                if (!hasSpecies)
                {
                    return (scientificNameParts[0], null, scientificNameParts[1], null, null, null);
                }
                else
                {
                    return (scientificNameParts[0], scientificNameParts[1], null, null, null, null);
                }
            }
            else
            {
                throw new NotSupportedException($"ScientificNameWithAuthor format is not supported: {scientificNameWithAuthor}");
            }
        }
    }
}
