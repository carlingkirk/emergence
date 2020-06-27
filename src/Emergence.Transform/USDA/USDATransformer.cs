using Emergence.Data.External.USDA;
using Emergence.Data.Stores;
using Emergence.Transform.Data;
using Emergence.Transform.USDA;
using System;
using System.Linq;

namespace Emergence.Transform
{
    public class USDATransformer : ITransformer<Lifeform, Checklist>
    {
        public Origin Origin => new Origin
        {
            Id = 1,
            Name = "USDA Checklist",
            Description = "",
            Uri = new Uri("https://plants.sc.egov.usda.gov/dl_all.html")
        };

        public Lifeform Transform(Checklist source)
        {
            (string Genus, string Species, string Author, string Variant) = ChecklistParser.ParseScientificNameWithAuthor(source.ScientificNameWithAuthor);

            var origin = new Origin
            {
                ParentId = Origin.Id,
                Name = Author,
                ExternalId = source.Symbol,
                AltExternalId = source.SynonymSymbol
            };

            return new Lifeform
            {
                Origin = origin,
                PlantInfo = new PlantInfo
                {
                    CommonName = source.CommonName,
                    ScientificName = Genus + " " + Species
                },
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
