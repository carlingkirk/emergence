using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.External.NatureServe;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;

namespace Emergence.Transform.NatureServe
{

    public class NatureServeTransformer
    {
        private string UrlRoot => "https://explorer.natureserve.org";
        private readonly Dictionary<string, string> _nations;

        public Origin Origin => new Origin
        {
            OriginId = 160832,
            Type = OriginType.Website
        };

        public NatureServeTransformer()
        {
            _nations = new Dictionary<string, string> { { "CA", "Canada" }, { "US", "USA" } };
        }

        public PlantInfo Transform(Plant source)
        {
            var dateCreated = DateTime.UtcNow;
            var species = source.ScientificName;

            if (string.IsNullOrEmpty(species))
            {
                return null;
            }

            if (species.Contains(" x "))
            {
                species = species.Replace(" x ", " ×");
            }

            var speciesParts = species.Split(' ');
            var scientificName = speciesParts[0] + " " + speciesParts[1];
            var speciesName = speciesParts[1];
            string variety = null;
            string subspecies = null;

            if (speciesParts.Count() > 2)
            {
                if (speciesParts.Any(species => species == "var."))
                {
                    variety = speciesParts.Last();
                }

                if (speciesParts.Any(species => species == "ssp."))
                {
                    subspecies = speciesParts.Last();
                }
            }

            var origin = new Origin
            {
                ParentOrigin = new Origin { OriginId = Origin.OriginId },
                Name = "NatureServe Plants Profile for " + species,
                Description = "NatureServe. 2021. NatureServe Explorer [web application]. NatureServe, Arlington, Virginia. Available https://explorer.natureserve.org/. (Accessed: Month 03, 2021).",
                ExternalId = source.UniqueId,
                AltExternalId = source.ElementGlobalId,
                Type = OriginType.Database,
                Uri = new Uri(UrlRoot + source.NsxUrl),
                DateCreated = dateCreated
            };

            var lifeform = new Lifeform
            {
                ScientificName = species,
                DateCreated = dateCreated
            };

            var plantInfo = new PlantInfo
            {
                ScientificName = scientificName,
                CommonName = source.PrimaryCommonName,
                Origin = origin,
                Lifeform = lifeform,
                Taxon = new Taxon
                {
                    Kingdom = source.SpeciesGlobal.Kingdom.NullIfEmpty(),
                    Phylum = source.SpeciesGlobal.Phylum.NullIfEmpty(),
                    Class = source.SpeciesGlobal.Taxclass.NullIfEmpty(),
                    Order = source.SpeciesGlobal.Taxorder.NullIfEmpty(),
                    Family = source.SpeciesGlobal.Family.NullIfEmpty(),
                    Genus = source.SpeciesGlobal.Genus.NullIfEmpty(),
                    Species = speciesName,
                    Subspecies = subspecies,
                    Variety = variety,
                    DateCreated = dateCreated
                },
                DateCreated = dateCreated
            };

            var locations = source.Nations.SelectMany(n => n.Subnations, (n, sn) => new { n, sn }).Select(nsn => new PlantLocation
            {
                Location = new Location { Country = _nations[nsn.n.NationCode], StateOrProvince = nsn.sn.SubnationCode },
                PlantInfo = plantInfo,
                Status = GetLocationStatus(nsn.sn),
                ConservationStatus = GetConservationStatus(nsn.sn.RoundedSRank)
            }).ToList();

            locations.AddRange(source.Nations.Select(n => new PlantLocation
            {
                Location = new Location { Country = _nations[n.NationCode] },
                PlantInfo = plantInfo,
                Status = GetLocationStatus(n),
                ConservationStatus = GetConservationStatus(n.RoundedNRank)
            }));

            plantInfo.Locations = locations;

            return plantInfo;
        }

        private ConservationStatus GetConservationStatus(string rank)
        {
            rank = rank[1..];

            switch (rank)
            {
                case "1":
                    return ConservationStatus.CriticallyEndangered;
                case "2":
                    return ConservationStatus.Endangered;
                case "3":
                    return ConservationStatus.Vulnerable;
                case "4":
                    return ConservationStatus.NearThreatened;
                case "5":
                    return ConservationStatus.LeastConcern;
                case "NR":
                    return ConservationStatus.NotEvaluated;
                case "U":
                    return ConservationStatus.DataDeficient;
                case "X":
                    return ConservationStatus.PresumedExtirpated;
                case "H":
                    return ConservationStatus.PossiblyExtirpated;
                case "NA":
                    return ConservationStatus.NotApplicable;
                default:
                    return ConservationStatus.NotEvaluated;
            }
        }

        private LocationStatus GetLocationStatus(Nation nation)
        {
            if (nation.Native)
            {
                return LocationStatus.Native;
            }
            else if (nation.Exotic)
            {
                return LocationStatus.Introduced;
            }

            return LocationStatus.Unknown;
        }

        private static LocationStatus GetLocationStatus(Subnation subnation)
        {
            if (subnation.Native)
            {
                return LocationStatus.Native;
            }
            else if (subnation.Exotic)
            {
                return LocationStatus.Introduced;
            }

            return LocationStatus.Unknown;
        }
    }
}
