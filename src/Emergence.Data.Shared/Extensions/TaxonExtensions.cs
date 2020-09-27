using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Enums;
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

        public static Models.Taxon Copy(this Models.Taxon source) => new Models.Taxon
        {
            Kingdom = !string.IsNullOrEmpty(source.Kingdom) ? string.Copy(source.Kingdom) : null,
            Infrakingdom = !string.IsNullOrEmpty(source.Infrakingdom) ? string.Copy(source.Infrakingdom) : null,
            Subkingdom = !string.IsNullOrEmpty(source.Subkingdom) ? string.Copy(source.Subkingdom) : null,
            Phylum = !string.IsNullOrEmpty(source.Phylum) ? string.Copy(source.Phylum) : null,
            Subphylum = !string.IsNullOrEmpty(source.Subphylum) ? string.Copy(source.Subphylum) : null,
            Superclass = !string.IsNullOrEmpty(source.Superclass) ? string.Copy(source.Superclass) : null,
            Class = !string.IsNullOrEmpty(source.Class) ? string.Copy(source.Class) : null,
            Subclass = !string.IsNullOrEmpty(source.Subclass) ? string.Copy(source.Subclass) : null,
            Infraclass = !string.IsNullOrEmpty(source.Infraclass) ? string.Copy(source.Infraclass) : null,
            Superorder = !string.IsNullOrEmpty(source.Superorder) ? string.Copy(source.Superorder) : null,
            Order = !string.IsNullOrEmpty(source.Order) ? string.Copy(source.Order) : null,
            Suborder = !string.IsNullOrEmpty(source.Suborder) ? string.Copy(source.Suborder) : null,
            Infraorder = !string.IsNullOrEmpty(source.Infraorder) ? string.Copy(source.Infraorder) : null,
            Epifamily = !string.IsNullOrEmpty(source.Epifamily) ? string.Copy(source.Epifamily) : null,
            Superfamily = !string.IsNullOrEmpty(source.Superfamily) ? string.Copy(source.Superfamily) : null,
            Family = !string.IsNullOrEmpty(source.Family) ? string.Copy(source.Family) : null,
            Subfamily = !string.IsNullOrEmpty(source.Subfamily) ? string.Copy(source.Subfamily) : null,
            Supertribe = !string.IsNullOrEmpty(source.Supertribe) ? string.Copy(source.Supertribe) : null,
            Tribe = !string.IsNullOrEmpty(source.Tribe) ? string.Copy(source.Tribe) : null,
            Subtribe = !string.IsNullOrEmpty(source.Subtribe) ? string.Copy(source.Subtribe) : null,
            GenusHybrid = !string.IsNullOrEmpty(source.GenusHybrid) ? string.Copy(source.GenusHybrid) : null,
            Genus = !string.IsNullOrEmpty(source.Genus) ? string.Copy(source.Genus) : null,
            Subgenus = !string.IsNullOrEmpty(source.Subgenus) ? string.Copy(source.Subgenus) : null,
            Section = !string.IsNullOrEmpty(source.Section) ? string.Copy(source.Section) : null,
            Hybrid = !string.IsNullOrEmpty(source.Hybrid) ? string.Copy(source.Hybrid) : null,
            Species = !string.IsNullOrEmpty(source.Species) ? string.Copy(source.Species) : null,
            Subspecies = !string.IsNullOrEmpty(source.Subspecies) ? string.Copy(source.Subspecies) : null,
            Variety = !string.IsNullOrEmpty(source.Variety) ? string.Copy(source.Variety) : null,
            Subvariety = !string.IsNullOrEmpty(source.Subvariety) ? string.Copy(source.Subvariety) : null,
            Form = !string.IsNullOrEmpty(source.Form) ? string.Copy(source.Form) : null,
            CreatedBy = !string.IsNullOrEmpty(source.CreatedBy) ? string.Copy(source.CreatedBy) : null,
            ModifiedBy = !string.IsNullOrEmpty(source.ModifiedBy) ? string.Copy(source.ModifiedBy) : null,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static string GetTaxonName(this Models.Taxon taxon, TaxonRank rank)
        {
            switch (rank)
            {
                case TaxonRank.Kingdom:
                    return taxon.Kingdom;
                case TaxonRank.Subkingdom:
                    return taxon.Subkingdom ?? "None";
                case TaxonRank.Infrakingdom:
                    return taxon.Infrakingdom ?? "None";
                case TaxonRank.Phylum:
                    return taxon.Phylum ?? "None";
                case TaxonRank.Subphylum:
                    return taxon.Subphylum ?? "None";
                case TaxonRank.Class:
                    return taxon.Class ?? "None";
                case TaxonRank.Subclass:
                    return taxon.Subclass ?? "None";
                case TaxonRank.Superorder:
                    return taxon.Superorder ?? "None";
                case TaxonRank.Order:
                    return taxon.Order ?? "None";
                case TaxonRank.Suborder:
                    return taxon.Suborder ?? "None";
                case TaxonRank.Family:
                    return taxon.Family ?? "None";
                case TaxonRank.Subfamily:
                    return taxon.Subfamily ?? "None";
                case TaxonRank.Genus:
                    return taxon.Genus ?? "None";
                case TaxonRank.Species:
                    return taxon.Species ?? "None";
                default:
                    return "None";
            }
        }

        public static TaxonRank GetParentRank(this TaxonRank rank)
        {
            switch (rank)
            {
                case TaxonRank.Subkingdom:
                    return TaxonRank.Kingdom;
                case TaxonRank.Infrakingdom:
                    return TaxonRank.Subkingdom;
                case TaxonRank.Phylum:
                    return TaxonRank.Infrakingdom;
                case TaxonRank.Subphylum:
                    return TaxonRank.Phylum;
                case TaxonRank.Class:
                    return TaxonRank.Subphylum;
                case TaxonRank.Subclass:
                    return TaxonRank.Class;
                case TaxonRank.Superorder:
                    return TaxonRank.Subclass;
                case TaxonRank.Order:
                    return TaxonRank.Superorder;
                case TaxonRank.Suborder:
                    return TaxonRank.Order;
                case TaxonRank.Family:
                    return TaxonRank.Suborder;
                case TaxonRank.Subfamily:
                    return TaxonRank.Family;
                case TaxonRank.Genus:
                    return TaxonRank.Subfamily;
                case TaxonRank.Species:
                    return TaxonRank.Genus;
                default:
                    return TaxonRank.Root;
            }
        }

        public static TaxonRank GetChildRank(this TaxonRank rank)
        {
            switch (rank)
            {
                case TaxonRank.Kingdom:
                    return TaxonRank.Subkingdom;
                case TaxonRank.Subkingdom:
                    return TaxonRank.Infrakingdom;
                case TaxonRank.Infrakingdom:
                    return TaxonRank.Phylum;
                case TaxonRank.Phylum:
                    return TaxonRank.Subphylum;
                case TaxonRank.Subphylum:
                    return TaxonRank.Class;
                case TaxonRank.Class:
                    return TaxonRank.Subclass;
                case TaxonRank.Subclass:
                    return TaxonRank.Superorder;
                case TaxonRank.Superorder:
                    return TaxonRank.Order;
                case TaxonRank.Order:
                    return TaxonRank.Suborder;
                case TaxonRank.Suborder:
                    return TaxonRank.Family;
                case TaxonRank.Family:
                    return TaxonRank.Subfamily;
                case TaxonRank.Subfamily:
                    return TaxonRank.Genus;
                case TaxonRank.Genus:
                    return TaxonRank.Species;
                case TaxonRank.Species:
                    return TaxonRank.Subspecies;
                default:
                    return TaxonRank.Root;
            }
        }
    }
}
