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
            OriginId = 1,
            Name = "USDA Checklist",
            Description = "",
            Uri = new Uri("https://plants.sc.egov.usda.gov/dl_all.html")
        };

        public PlantInfo Transform(Checklist source)
        {
            (var Genus, var Species, var Author, var Variant) = ChecklistParser.ParseScientificNameWithAuthor(source.ScientificNameWithAuthor);

            var origin = new Origin
            {
                ParentOrigin = new Origin { OriginId = Origin.OriginId },
                Name = Author,
                ExternalId = source.Symbol,
                AltExternalId = source.SynonymSymbol
            };

            var lifeform = new Lifeform
            {
                CommonName = source.CommonName,
                ScientificName = Genus + " " + Species
            };

            return new PlantInfo
            {
                CommonName = source.CommonName,
                ScientificName = Genus + " " + Species,
                Origin = origin,
                Lifeform = lifeform,
                Taxon = new Taxon
                {
                    Family = source.Family,
                    Genus = Genus,
                    Species = Species,
                    Variety = Variant
                }
            };
        }
    }
}
