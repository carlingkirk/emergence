using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client
{
    public abstract class ListComponent<T> : ComponentBase, ISortable<T>, ISearchable<T>, IPageable<T> where T : class
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        [Inject]
        protected IModalServiceClient ModalServiceClient { get; set; }
        public IEnumerable<T> List { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        public int Take { get; set; }
        public int Count { get; set; }
        public abstract Task<FindResult<T>> GetListAsync(string searchText, int? skip = 0, int? take = 10, string sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending);

        protected override async Task OnInitializedAsync()
        {
            CurrentPage = 1;
            Take = 10;
            await FindAsync();
        }

        public async Task FindAsync()
        {
            var skip = (CurrentPage - 1) * Take;
            var result = await GetListAsync(SearchText, skip, Take, SortBy, SortDirection);
            List = result.Results;
            Count = result.Count;
        }

        protected async Task<IEnumerable<T>> SortAsync(string sortBy, SortDirection sortDirection)
        {
            SortDirection = sortDirection;
            SortBy = sortBy;
            await FindAsync();
            return List;
        }

        protected async Task<IEnumerable<T>> PageAsync(int page)
        {
            CurrentPage = page;
            await FindAsync();
            return List;
        }
    }
}
