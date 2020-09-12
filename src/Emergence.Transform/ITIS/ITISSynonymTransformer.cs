using System.Linq;
using Emergence.Data.External.USDA;
using Emergence.Data.Shared;
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

        public Synonym Transform(Vernacular source) => new Synonym
        {
            Name = source.Name,
            Rank = source.Rank,
            Language = source.Language,
            DateUpdated = source.DateUpdated,
            Origin = new Origin
            {
                ExternalId = source.Tsn,
                ParentOrigin = Origin
            },
            Taxon = GetTaxon(source.Rank, source.Taxon)
        };

        private Taxon GetTaxon(string rank, string name)
        {
            if (name.Contains(' '))
            {
                name = name.Split(' ').Last();
            }

            switch (rank)
            {
                case "Kingdom":
                    return new Taxon { Kingdom = name };
                case "Subkingdom":
                    return new Taxon { Subkingdom = name };
                case "Infrakingdom":
                    return new Taxon { Infrakingdom = name };
                case "Phylum":
                    return new Taxon { Phylum = name };
                case "Subphylum":
                    return new Taxon { Subphylum = name };
                case "Class":
                    return new Taxon { Class = name };
                case "Subclass":
                    return new Taxon { Subclass = name };
                case "Superorder":
                    return new Taxon { Superorder = name };
                case "Order":
                    return new Taxon { Order = name };
                case "Family":
                    return new Taxon { Family = name };
                case "Genus":
                    return new Taxon { Genus = name };
                case "Species":
                    return new Taxon { Species = name };
                case "Subspecies":
                    return new Taxon { Subspecies = name };
                case "Variety":
                    return new Taxon { Variety = name };
                case "Form":
                    return new Taxon { Form = name };
                default:
                    return null;
            }
        }
    }
}
