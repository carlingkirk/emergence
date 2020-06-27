using Emergence.Data.External.iNaturalist;
using Emergence.Data.Shared.Stores;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Emergence.Transform.Data
{
    public class iNaturalistPlantInfo : ITransformer<Lifeform, Observation>
    {
        public Origin Origin => new Origin
        {
            Id = 0,
            Name = "iNaturalist",
            Description = "iNaturalist is an online social network of people sharing biodiversity information to help each other learn about nature",
            Uri = new Uri("https://www.inaturalist.org/")
        };

        public Lifeform Transform(Observation source)
        {
            var origin = new Origin
            {
                ParentId = Origin.Id,
                Uri = new Uri(source.uri)
            };
            return new Lifeform
            {
                PlantInfo = new PlantInfo
                {
                    CommonName = source.taxon?.preferred_common_name,
                    OriginId = Origin.Id
                },
                Taxon = GetFullTaxon(source),
                Origin = origin
            };
        }

        public static Emergence.Data.Shared.Stores.Taxon GetFullTaxon(Observation observation)
        {
            var bestId = observation.identifications?.Where(id => id.own_observation).FirstOrDefault() ?? observation.identifications?.FirstOrDefault();
            var ancestors = bestId?.taxon?.ancestors?.ToList();
            var taxon = new Emergence.Data.Shared.Stores.Taxon
            {
                Kingdom = GetAncestor(ancestors, Rank.Kingdom)?.name,
                Phylum = GetAncestor(ancestors, Rank.Phylum)?.name,
                Subphylum = GetAncestor(ancestors, Rank.Subphylum)?.name,
                Superclass = GetAncestor(ancestors, Rank.Superclass)?.name,
                Class = GetAncestor(ancestors, Rank.Class)?.name,
                Subclass = GetAncestor(ancestors, Rank.Subclass)?.name,
                Infraclass = GetAncestor(ancestors, Rank.Infraclass)?.name,
                Superorder = GetAncestor(ancestors, Rank.Superorder)?.name,
                Order = GetAncestor(ancestors, Rank.Order)?.name,
                Suborder = GetAncestor(ancestors, Rank.Suborder)?.name,
                Infraorder = GetAncestor(ancestors, Rank.Infraorder)?.name,
                Epifamily = GetAncestor(ancestors, Rank.Epifamily)?.name,
                Superfamily = GetAncestor(ancestors, Rank.Superfamily)?.name,
                Family = GetAncestor(ancestors, Rank.Family)?.name,
                Subfamily = GetAncestor(ancestors, Rank.Subfamily)?.name,
                Supertribe = GetAncestor(ancestors, Rank.Supertribe)?.name,
                Tribe = GetAncestor(ancestors, Rank.Tribe)?.name,
                Subtribe = GetAncestor(ancestors, Rank.Subtribe)?.name,
                GenusHybrid = GetAncestor(ancestors, Rank.GenusHybrid)?.name,
                Genus = GetAncestor(ancestors, Rank.Genus)?.name,
                Subgenus = GetAncestor(ancestors, Rank.Subgenus)?.name,
                Hybrid = GetAncestor(ancestors, Rank.Hybrid)?.name,
                Species = GetAncestor(ancestors, Rank.Species)?.name,
                Subspecies = GetAncestor(ancestors, Rank.Subspecies)?.name,
                Variety = GetAncestor(ancestors, Rank.Variety)?.name,
                Form = GetAncestor(ancestors, Rank.Form)?.name,
                Section = GetAncestor(ancestors, Rank.Section)?.name
            };

            return taxon;
        }

        private static Ancestor GetAncestor(IEnumerable<Ancestor> ancestors, Rank rank)
        {
            return ancestors?.Where(a => a.rank == rank).FirstOrDefault();
        }
    }
}
