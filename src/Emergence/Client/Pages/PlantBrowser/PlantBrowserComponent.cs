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
            ShowSearch = false;

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
                    break;
                case TaxonRank.Subkingdom:
                    Shape.Subkingdom = name;
                    break;
                case TaxonRank.Infrakingdom:
                    Shape.Infrakingdom = name;
                    break;
                case TaxonRank.Phylum:
                    Shape.Phylum = name;
                    break;
                case TaxonRank.Subphylum:
                    Shape.Subphylum = name;
                    break;
                case TaxonRank.Class:
                    Shape.Class = name;
                    break;
                case TaxonRank.Subclass:
                    Shape.Subclass = name;
                    break;
                case TaxonRank.Superorder:
                    Shape.Superorder = name;
                    break;
                case TaxonRank.Order:
                    Shape.Order = name;
                    break;
                case TaxonRank.Suborder:
                    Shape.Suborder = name;
                    break;
                case TaxonRank.Family:
                    Shape.Family = name;
                    break;
                case TaxonRank.Subfamily:
                    Shape.Subfamily = name;
                    break;
                case TaxonRank.Genus:
                    Shape.Genus = name;
                    break;
                case TaxonRank.Species:
                    Shape.Species = name;
                    break;
            }

            Rank = rank.GetChildRank();

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
            Rank = rank.GetChildRank();
            foreach (var breadcrumb in Breadcrumbs)
            {
                Console.WriteLine(breadcrumb.GetHashCode());
            }

            Console.WriteLine(Shape.GetHashCode());
            await SearchAsync();
        }
    }
}
