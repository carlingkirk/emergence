using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Enums;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class PlantBrowserComponent : ListComponent<Taxon>
    {
        public IEnumerable<Taxon> Taxons { get; set; }
        public Taxon Shape { get; set; }

        public TaxonRank Rank { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentPage = 1;
            Take = 10;
            Rank = TaxonRank.Kingdom;

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

            FindResult<Taxon> result;

            result = await ApiClient.FindTaxonsAsync(findTaxonParams);

            return result;
        }

        public async Task SearchAsync(TaxonRank rank, string name)
        {
            switch (rank)
            {
                case TaxonRank.Kingdom:
                    Shape = new Taxon { Kingdom = name };
                    Rank = TaxonRank.Subkingdom;
                    break;
                case TaxonRank.Subkingdom:
                    Shape = new Taxon { Subkingdom = name };
                    Rank = TaxonRank.Infrakingdom;
                    break;
                case TaxonRank.Infrakingdom:
                    Shape = new Taxon { Infrakingdom = name };
                    Rank = TaxonRank.Phylum;
                    break;
                case TaxonRank.Phylum:
                    Shape = new Taxon { Phylum = name };
                    Rank = TaxonRank.Subphylum;
                    break;
                case TaxonRank.Subphylum:
                    Shape = new Taxon { Subphylum = name };
                    Rank = TaxonRank.Class;
                    break;
                case TaxonRank.Class:
                    Shape = new Taxon { Class = name };
                    Rank = TaxonRank.Subclass;
                    break;
                case TaxonRank.Subclass:
                    Shape = new Taxon { Subclass = name };
                    Rank = TaxonRank.Superorder;
                    break;
                case TaxonRank.Superorder:
                    Shape = new Taxon { Superorder = name };
                    Rank = TaxonRank.Order;
                    break;
                case TaxonRank.Order:
                    Shape = new Taxon { Order = name };
                    Rank = TaxonRank.Suborder;
                    break;
                case TaxonRank.Suborder:
                    Shape = new Taxon { Suborder = name };
                    Rank = TaxonRank.Family;
                    break;
                case TaxonRank.Family:
                    Shape = new Taxon { Family = name };
                    Rank = TaxonRank.Subfamily;
                    break;
                case TaxonRank.Subfamily:
                    Shape = new Taxon { Subfamily = name };
                    Rank = TaxonRank.Genus;
                    break;
                case TaxonRank.Genus:
                    Shape = new Taxon { Genus = name };
                    Rank = TaxonRank.Species;
                    break;
                case TaxonRank.Species:
                    Shape = new Taxon { Species = name };
                    Rank = TaxonRank.Subspecies;
                    break;
            }

            await SearchAsync();
        }
    }
}
