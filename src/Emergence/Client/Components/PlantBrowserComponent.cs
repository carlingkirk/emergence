using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class PlantBrowserComponent : ListComponent<Taxon>
    {
        public IEnumerable<Taxon> Taxons { get; set; }
        public Taxon Shape { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            CurrentPage = 1;
            Take = 10;
            Shape = new Taxon
            {
                Kingdom = ""
            };
            await FindAsync();
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
    }
}
