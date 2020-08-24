using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class TaxonExtensions
    {
        public static Models.Taxon AsModel(this Taxon source) => new Models.Taxon
        {
            TaxonId = source.Id,
            Kingdom = source.Kingdom,
            Phylum = source.Phylum,
            Subphylum = source.Subphylum,
            Superclass = source.Superclass,
            Class = source.Class,
            Subclass = source.Subclass,
            Infraclass = source.Infraclass,
            Superorder = source.Superorder,
            Order = source.Order,
            Suborder = source.Suborder,
            Infraorder = source.Infraorder,
            Epifamily = source.Epifamily,
            Superfamily = source.Superfamily,
            Family = source.Family,
            Subfamily = source.Subfamily,
            Supertribe = source.Supertribe,
            Tribe = source.Tribe,
            Subtribe = source.Subtribe,
            GenusHybrid = source.GenusHybrid,
            Genus = source.Genus,
            Subgenus = source.Subgenus,
            Section = source.Section,
            Hybrid = source.Hybrid,
            Species = source.Species,
            Subspecies = source.Subspecies,
            Variety = source.Variety,
            Subvariety = source.Subvariety,
            Form = source.Form,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Taxon AsStore(this Models.Taxon source) => new Taxon
        {
            Id = source.TaxonId,
            Kingdom = source.Kingdom,
            Phylum = source.Phylum,
            Subphylum = source.Subphylum,
            Superclass = source.Superclass,
            Class = source.Class,
            Subclass = source.Subclass,
            Infraclass = source.Infraclass,
            Superorder = source.Superorder,
            Order = source.Order,
            Suborder = source.Suborder,
            Infraorder = source.Infraorder,
            Epifamily = source.Epifamily,
            Superfamily = source.Superfamily,
            Family = source.Family,
            Subfamily = source.Subfamily,
            Supertribe = source.Supertribe,
            Tribe = source.Tribe,
            Subtribe = source.Subtribe,
            GenusHybrid = source.GenusHybrid,
            Genus = source.Genus,
            Subgenus = source.Subgenus,
            Section = source.Section,
            Hybrid = source.Hybrid,
            Species = source.Species,
            Subspecies = source.Subspecies,
            Variety = source.Variety,
            Subvariety = source.Subvariety,
            Form = source.Form,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
