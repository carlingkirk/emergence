using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Enums;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class PlantBrowserComponent : ListComponent<Taxon>
    {
        public IEnumerable<Taxon> Taxons { get; set; }
        public Taxon Shape { get; set; }
        public Dictionary<TaxonRank, Taxon> Breadcrumbs { get; set; }

        public TaxonRank Rank { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentPage = 1;
            Take = 10;
            Rank = TaxonRank.Subkingdom;
            Shape = new Taxon { Kingdom = "Plantae" };
            Breadcrumbs = new Dictionary<TaxonRank, Taxon>
            {
                {
                    TaxonRank.Kingdom,
                    Shape.Copy()
                }
            };

            await base.OnInitializedAsync();
        }

        public async override Task<FindResult<Taxon>> GetListAsync(FindParams findParams)
        {
            var findTaxonParams = new FindParams<Taxon>
            {
                SearchText = findParams.SearchText,
                Skip = findParams.Skip,
                Take = findParams.Take,
                SortBy = findParams.SortBy,
                SortDirection = findParams.SortDirection,
                Shape = Shape
            };

            var result = await ApiClient.FindTaxonsAsync(findTaxonParams, Rank);

            return result;
        }

        public async Task SearchAsync(TaxonRank rank, string name)
        {
            if (Shape != null && !Breadcrumbs.ContainsKey(rank))
            {
                Breadcrumbs.Add(Rank, Shape.Copy());
            }

            switch (rank)
            {
                case TaxonRank.Kingdom:
                    Shape.Kingdom = name;
                    Rank = TaxonRank.Subkingdom;
                    break;
                case TaxonRank.Subkingdom:
                    Shape.Subkingdom = name;
                    Rank = TaxonRank.Infrakingdom;
                    break;
                case TaxonRank.Infrakingdom:
                    Shape.Infrakingdom = name;
                    Rank = TaxonRank.Phylum;
                    break;
                case TaxonRank.Phylum:
                    Shape.Phylum = name;
                    Rank = TaxonRank.Subphylum;
                    break;
                case TaxonRank.Subphylum:
                    Shape.Subphylum = name;
                    Rank = TaxonRank.Class;
                    break;
                case TaxonRank.Class:
                    Shape.Class = name;
                    Rank = TaxonRank.Subclass;
                    break;
                case TaxonRank.Subclass:
                    Shape.Subclass = name;
                    Rank = TaxonRank.Superorder;
                    break;
                case TaxonRank.Superorder:
                    Shape.Superorder = name;
                    Rank = TaxonRank.Order;
                    break;
                case TaxonRank.Order:
                    Shape.Order = name;
                    Rank = TaxonRank.Suborder;
                    break;
                case TaxonRank.Suborder:
                    Shape.Suborder = name;
                    Rank = TaxonRank.Family;
                    break;
                case TaxonRank.Family:
                    Shape.Family = name;
                    Rank = TaxonRank.Subfamily;
                    break;
                case TaxonRank.Subfamily:
                    Shape.Subfamily = name;
                    Rank = TaxonRank.Genus;
                    break;
                case TaxonRank.Genus:
                    Shape.Genus = name;
                    Rank = TaxonRank.Species;
                    break;
                case TaxonRank.Species:
                    Shape.Species = name;
                    Rank = TaxonRank.Subspecies;
                    break;
            }

            Breadcrumbs[rank] = Shape.Copy();

            await SearchAsync();
        }

        public async Task NavigateAsync(TaxonRank rank)
        {
            // we click on class we should get subclass
            // currently getting class
            var crumb = Breadcrumbs[rank];
            Breadcrumbs = Breadcrumbs.Where(b => b.Key <= rank).ToDictionary(b => b.Key, b => b.Value);
            Shape = crumb.Copy();
            Rank = GetChildRank(rank);
            foreach (var breadcrumb in Breadcrumbs)
            {
                Console.WriteLine(breadcrumb.GetHashCode());
            }

            Console.WriteLine(Shape.GetHashCode());
            await SearchAsync();
        }

        protected string GetTaxonName(TaxonRank rank, Taxon taxon)
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

        protected TaxonRank GetParentRank(TaxonRank rank)
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

        protected TaxonRank GetChildRank(TaxonRank rank)
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
