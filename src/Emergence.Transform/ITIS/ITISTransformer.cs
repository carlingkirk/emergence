using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.External.ITIS;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;

namespace Emergence.Transform
{
    public class ITISTransformer : ITransformer<IEnumerable<PlantInfo>, IEnumerable<TaxonomicUnit>>
    {
        public Origin Origin => new Origin
        {
            OriginId = 89983,
            Type = OriginType.Database
        };

        public IEnumerable<PlantInfo> Transform(IEnumerable<TaxonomicUnit> sources)
        {
            var plantInfos = new List<PlantInfo>();
            var source = sources.First();
            var dateCreated = DateTime.UtcNow;
            var species = source.Species ?? source.Subspecies ?? source.Variety ?? source.Form;

            if (string.IsNullOrEmpty(species))
            {
                return plantInfos;
            }

            var speciesParts = species.Split(' ');
            var scientificName = speciesParts[0] + " " + speciesParts[1];
            var speciesName = speciesParts[1];

            if (!string.IsNullOrEmpty(source.Subspecies))
            {
                source.Subspecies = source.Subspecies.NullIfEmpty().Split(' ').Last();
            }

            if (!string.IsNullOrEmpty(source.Variety))
            {
                source.Variety = source.Variety.NullIfEmpty().Split(' ').Last();
            }

            if (!string.IsNullOrEmpty(source.Form))
            {
                source.Form = source.Form.NullIfEmpty().Split(' ').Last();
            }

            var origin = new Origin
            {
                ParentOrigin = new Origin { OriginId = Origin.OriginId },
                Name = "ITIS Plants Profile for " + species,
                Authors = source.Author.NullIfEmpty(),
                ExternalId = source.Tsn,
                Type = OriginType.Database,
                DateCreated = dateCreated
            };

            var lifeform = new Lifeform
            {
                ScientificName = species,
                DateCreated = dateCreated
            };

            var plantInfo = new PlantInfo
            {
                ScientificName = source.Species,
                Origin = origin,
                Lifeform = lifeform,
                Taxon = new Taxon
                {
                    Kingdom = source.Kingdom.NullIfEmpty(),
                    Subkingdom = source.Subkingdom.NullIfEmpty(),
                    Infrakingdom = source.Infrakingdom.NullIfEmpty(),
                    Superphylum = source.Superdivision.NullIfEmpty(),
                    Phylum = source.Division.NullIfEmpty(),
                    Subphylum = source.Subdivision.NullIfEmpty(),
                    Class = source.Class.NullIfEmpty(),
                    Subclass = source.Subclass.NullIfEmpty(),
                    Superorder = source.Superorder.NullIfEmpty(),
                    Order = source.Order.NullIfEmpty(),
                    Suborder = source.Suborder.NullIfEmpty(),
                    Family = source.Family.NullIfEmpty(),
                    Subfamily = source.Subfamily.NullIfEmpty(),
                    Genus = source.Genus.NullIfEmpty(),
                    Subgenus = source.Subgenus.NullIfEmpty(),
                    Species = speciesName,
                    Subspecies = source.Subspecies.NullIfEmpty(),
                    Variety = source.Variety.NullIfEmpty(),
                    Form = source.Form.NullIfEmpty(),
                    DateCreated = dateCreated
                },
                DateCreated = dateCreated
            };

            plantInfo.Locations = sources.Where(s => !string.IsNullOrEmpty(s.Region) || !string.IsNullOrEmpty(s.Country)).Select(l => new PlantLocation
            {
                Location = new Location { Country = l.Country.NullIfEmpty(), Region = l.Region.NullIfEmpty() },
                PlantInfo = plantInfo,
                Status = GetLocationStatus(l.LocationStatus)
            });

            plantInfos.Add(plantInfo);

            return plantInfos;
        }

        private static LocationStatus GetLocationStatus(string status) => status switch
        {
            "Native" => LocationStatus.Native,
            "Incidental" => LocationStatus.Incidental,
            "Introduced" => LocationStatus.Introduced,
            "Native & Introduced" => LocationStatus.NativeIntroduced,
            "Native & Extirpated" => LocationStatus.NativeExtirpated,
            _ => LocationStatus.Unknown,
        };
    }
}
