using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class TaxonExtensions
    {
        public static Models.Taxon AsModel(this Taxon source) => new Models.Taxon
        {
            TaxonId = source.Id,
            Kingdom = source.Kingdom,
            Infrakingdom = source.Infrakingdom,
            Subkingdom = source.Subkingdom,
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
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
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
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };

        public static Models.Taxon GetTaxon(this IEnumerable<Models.Taxon> source, Taxon taxon) =>
            source.FirstOrDefault(t => t.Kingdom == taxon.Kingdom
                                    && t.Subkingdom == taxon.Subkingdom
                                    && t.Infrakingdom == taxon.Infrakingdom
                                    && t.Phylum == taxon.Phylum
                                    && t.Subphylum == taxon.Subphylum
                                    && t.Class == taxon.Class
                                    && t.Subclass == taxon.Subclass
                                    && t.Order == taxon.Order
                                    && t.Family == taxon.Family
                                    && t.Genus == taxon.Genus
                                    && t.Species == taxon.Species
                                    && t.Subspecies == taxon.Variety
                                    && t.Subvariety == taxon.Subvariety
                                    && t.Form == taxon.Form);
    }
}
