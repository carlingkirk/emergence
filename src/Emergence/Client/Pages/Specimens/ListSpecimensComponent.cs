using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Emergence.Data.Shared.Search;

namespace Emergence.Client.Components
{
    public class ListSpecimensComponent : ListComponent<Specimen>
    {
        public SpecimenFilters SpecimenFilters { get; set; }
        protected static Dictionary<string, string> Headers =>
            new Dictionary<string, string>
            {
                { "ScientificName", "Scientific Name" },
                { "CommonName", "Common Name" },
                { "Quantity", "Quantity" },
                { "Stage", "Growth Stage" },
                { "Status", "Status" },
                { "DateAcquired", "Date Acquired" },
                { "Origin", "Origin" },
                { "", "" }
            };

        public override async Task<FindResult<Specimen>> GetListAsync(FindParams findParams)
        {
            var findSpecimenParams = new SpecimenFindParams
            {
                SearchText = findParams.SearchText,
                UseNGrams = false,
                Skip = findParams.Skip,
                Take = findParams.Take,
                SortBy = findParams.SortBy,
                SortDirection = findParams.SortDirection,
                Filters = SpecimenFilters,
                CreatedBy = findParams.CreatedBy
            };

            var result = await ApiClient.FindSpecimensAsync(findSpecimenParams);

            SpecimenFilters = result.Filters;

            return new SpecimenFindResult
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
