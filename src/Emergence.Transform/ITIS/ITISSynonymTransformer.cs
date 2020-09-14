using System;
using System.Linq;
using Emergence.Data.External.USDA;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;

namespace Emergence.Transform
{
    public class ITISSynonymTransformer : ITransformer<Synonym, Vernacular>
    {
        public Origin Origin => new Origin
        {
            OriginId = 89983,
            Type = OriginType.Database
        };

        public Synonym Transform(Vernacular source)
        {
            var dateCreated = DateTime.UtcNow;
            var species = source.Species ?? source.Subspecies ?? source.Variety ?? source.Form;
            var speciesParts = string.IsNullOrEmpty(species) ? null : species.Split(' ');
            var speciesName = string.IsNullOrEmpty(species) ? null : speciesParts[1];

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

            return new Synonym
            {
                Name = source.Name,
                Rank = source.Rank,
                Language = source.Language,
                DateUpdated = source.DateUpdated,
                Origin = new Origin
                {
                    ExternalId = source.Tsn,
                    ParentOrigin = Origin,
                    Name = "ITIS Plants Profile for " + source.Taxon,
                    Type = OriginType.Database,
                    DateCreated = dateCreated
                },
                Taxon = new Taxon
                {
                    Kingdom = source.Kingdom,
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
                }
            };
        }

        private Taxon GetTaxon(string rank, string name)
        {
            if (name.Contains(' '))
            {
                name = name.Split(' ').Last();
            }

            switch (rank)
            {
                case "Kingdom":
                    return new Taxon { Kingdom = name, DateCreated = DateTime.UtcNow };
                case "Subkingdom":
                    return new Taxon { Subkingdom = name, DateCreated = DateTime.UtcNow };
                case "Infrakingdom":
                    return new Taxon { Infrakingdom = name, DateCreated = DateTime.UtcNow };
                case "Phylum":
                    return new Taxon { Phylum = name, DateCreated = DateTime.UtcNow };
                case "Subphylum":
                    return new Taxon { Subphylum = name, DateCreated = DateTime.UtcNow };
                case "Class":
                    return new Taxon { Class = name, DateCreated = DateTime.UtcNow };
                case "Subclass":
                    return new Taxon { Subclass = name, DateCreated = DateTime.UtcNow };
                case "Superorder":
                    return new Taxon { Superorder = name, DateCreated = DateTime.UtcNow };
                case "Order":
                    return new Taxon { Order = name, DateCreated = DateTime.UtcNow };
                case "Family":
                    return new Taxon { Family = name, DateCreated = DateTime.UtcNow };
                case "Genus":
                    return new Taxon { Genus = name, DateCreated = DateTime.UtcNow };
                case "Species":
                    return new Taxon { Species = name, DateCreated = DateTime.UtcNow };
                case "Subspecies":
                    return new Taxon { Subspecies = name, DateCreated = DateTime.UtcNow };
                case "Variety":
                    return new Taxon { Variety = name, DateCreated = DateTime.UtcNow };
                case "Form":
                    return new Taxon { Form = name, DateCreated = DateTime.UtcNow };
                default:
                    return null;
            }
        }
    }
}
