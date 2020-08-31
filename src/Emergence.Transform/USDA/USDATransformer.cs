using System;
using Emergence.Data.External.USDA;
using Emergence.Data.Shared;
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
            Uri = new Uri("https://plants.sc.egov.usda.gov/dl_all.html"),
            Type = OriginType.File
        };

        public PlantInfo Transform(Checklist source)
        {
            (var genus, var species, var author, var subspecies, var variety, var subvariety) = ChecklistParser.ParseScientificNameWithAuthor(source.ScientificNameWithAuthor);
            var dateCreated = DateTime.UtcNow;
            var scientificName = species != null ? genus + " " + species : genus;

            var origin = new Origin
            {
                ParentOrigin = new Origin { OriginId = Origin.OriginId },
                Name = "USDA Plants Profile for " + scientificName,
                Authors = author,
                ExternalId = source.Symbol,
                AltExternalId = !string.IsNullOrEmpty(source.SynonymSymbol) ? source.SynonymSymbol : null,
                Type = OriginType.File,
                Uri = new Uri("https://plants.usda.gov/core/profile?symbol=" + source.Symbol),
                DateCreated = dateCreated
            };

            var lifeform = new Lifeform
            {
                CommonName = source.CommonName,
                ScientificName = scientificName,
                DateCreated = dateCreated
            };

            return new PlantInfo
            {
                CommonName = source.CommonName,
                ScientificName = scientificName,
                Origin = origin,
                Lifeform = lifeform,
                Taxon = new Taxon
                {
                    Family = source.Family,
                    Genus = genus,
                    Species = species,
                    Subspecies = subspecies,
                    Variety = variety,
                    Subvariety = subvariety,
                    DateCreated = dateCreated
                },
                DateCreated = dateCreated
            };
        }
    }
}
