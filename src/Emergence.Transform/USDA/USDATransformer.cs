using System;
using Emergence.Data.External.USDA;
using Emergence.Data.Shared.Models;
using Emergence.Transform.USDA;

namespace Emergence.Transform
{
    public class USDATransformer : ITransformer<PlantInfo, Checklist>
    {
        public Origin Origin => new Origin
        {
            OriginId = 2,
            Name = "USDA Checklist",
            Description = "",
            Uri = new Uri("https://plants.sc.egov.usda.gov/dl_all.html")
        };

        public PlantInfo Transform(Checklist source)
        {
            (var genus, var species, var author, var subspecies, var variety, var subvariety) = ChecklistParser.ParseScientificNameWithAuthor(source.ScientificNameWithAuthor);

            var origin = new Origin
            {
                ParentOrigin = new Origin { OriginId = Origin.OriginId },
                Name = author,
                ExternalId = source.Symbol,
                AltExternalId = source.SynonymSymbol
            };

            var lifeform = new Lifeform
            {
                CommonName = source.CommonName,
                ScientificName = genus + " " + species
            };

            return new PlantInfo
            {
                CommonName = source.CommonName,
                ScientificName = genus + " " + species,
                Origin = origin,
                Lifeform = lifeform,
                Taxon = new Taxon
                {
                    Family = source.Family,
                    Genus = genus,
                    Species = species,
                    Subspecies = subspecies,
                    Variety = variety,
                    Subvariety = subvariety
                }
            };
        }
    }
}
