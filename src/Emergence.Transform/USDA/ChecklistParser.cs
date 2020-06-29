using System;

namespace Emergence.Transform.USDA
{
    public static class ChecklistParser
    {
        public static (string Genus, string Species, string Author, string Variant) ParseScientificNameWithAuthor(string scientificNameWithAuthor)
        {
            var scientificNameParts = scientificNameWithAuthor.Split(" ");

            if (scientificNameParts.Length >= 3)
            {
                // It has genus, species and multiple author words
                var author = "";
                var variant = "";

                // Is it a var.? If so we don't care about the first author
                if (scientificNameWithAuthor.Contains(" var. "))
                {
                    byte? found = null;
                    for (byte i = 0; i < scientificNameParts.Length; i++)
                    {
                        if (found != null)
                        {
                            if (i == found + 1)
                            {
                                variant = scientificNameParts[i];
                            }
                            else
                            {
                                author += scientificNameParts[i] + " ";
                            }
                        }
                        else if (scientificNameParts[i] == "var.")
                        {
                            found = i;
                        }
                    }
                    return (scientificNameParts[0], scientificNameParts[1], author.Trim(), variant);
                }
                else
                {
                    for (var i = 2; i < scientificNameParts.Length; i++)
                    {
                        author += scientificNameParts[i] + " ";
                    }
                    return (scientificNameParts[0], scientificNameParts[1], author.Trim(), null);
                }
            }
            else if (scientificNameParts.Length == 2)
            {
                // Is the second word an author?
                if (Char.IsUpper(scientificNameParts[1][0]))
                {
                    return (scientificNameParts[0], null, scientificNameParts[1], null);
                }
                else
                {
                    return (scientificNameParts[0], scientificNameParts[1], null, null);
                }
            }
            else
            {
                throw new NotSupportedException($"ScientificNameWithAuthor format is not supported: {scientificNameWithAuthor}");
            }
        }
    }
}
