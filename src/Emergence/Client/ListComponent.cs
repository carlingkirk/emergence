using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client
{
    public abstract class ListComponent<T> : EmergenceComponent, ISortable<T>, ISearchable<T>, IPageable<T> where T : class
    {
        [Inject]
        protected IModalServiceClient ModalServiceClient { get; set; }
        [Parameter]
        public bool ShowSearch { get; set; }
        public IEnumerable<T> List { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        public int Take { get; set; }
        public int Count { get; set; }
        public bool IsItemLoaded { get; set; }
        public int Id { get; set; }

        public ListComponent()
        {
            ShowSearch = true;
        }

        public abstract Task<FindResult<T>> GetListAsync(FindParams findParams);

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            CurrentPage = 1;
            Take = 10;
            await FindAsync();
        }

        public async Task FindAsync()
        {
            var skip = (CurrentPage - 1) * Take;
            var result = await GetListAsync(new FindParams
            {
                SearchText = SearchText,
                Skip = skip,
                Take = Take,
                SortBy = SortBy,
                SortDirection = SortDirection
            });
            List = result.Results;
            Count = result.Count;
        }

        public async Task SearchAsync()
        {
            CurrentPage = 1;
            SortBy = null;
            SortDirection = SortDirection.Ascending;
            await FindAsync();
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

        protected void LoadInfo(int id)
        {
            Id = id;
            IsItemLoaded = true;
        }
    }
}
