using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Common
{
    public abstract class ListComponent<T> : EmergenceComponent, ISortable<T>, ISearchable<T>, IPageable<T>, IFilterable<T> where T : class
    {
        [Inject]
        protected IModalServiceClient ModalServiceClient { get; set; }
        [Parameter]
        public bool ShowSearch { get; set; }
        [Parameter]
        public bool LinkRelations { get; set; }
        [Parameter]
        public string ForUserId { get; set; }
        public IEnumerable<T> List { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        [Parameter]
        public int Take { get; set; }
        public long Count { get; set; }
        public bool ShowPublic { get; set; }
        public bool IsItemLoaded { get; set; }
        public bool ListView { get; set; }
        public int Id { get; set; }
        public T Parent { get; set; }
        public ViewItemType ViewItemType { get; set; }
        public bool ShowFilters { get; set; }

        protected ListComponent()
        {
            ShowSearch = true;
            LinkRelations = true;
        }

        public abstract Task<FindResult<T>> GetListAsync(FindParams findParams);

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            CurrentPage = 1;
            Take = Take == 0 ? 12 : Take;
            await FindAsync();
        }

        public async Task FindAsync()
        {
            var skip = (CurrentPage - 1) * Take;
            List = null;

            var result = await GetListAsync(new FindParams
            {
                SearchText = SearchText,
                Skip = skip,
                Take = Take,
                SortBy = SortBy,
                SortDirection = SortDirection,
                CreatedBy = ForUserId ?? (!ShowPublic ? UserId : ForUserId)
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

        protected async Task<IEnumerable<T>> PageAsync(int page, int perPage)
        {
            CurrentPage = page;
            Take = perPage;
            await FindAsync();
            return List;
        }

        protected async Task<IEnumerable<T>> RefreshAsync()
        {
            await FindAsync();

            if (!List.Any() && CurrentPage > 1)
            {
                CurrentPage--;
                await PageAsync(CurrentPage, Take);
            }

            return List;
        }

        protected void LoadInfo(ViewItemType type, int id)
        {
            Id = id;
            IsItemLoaded = true;
            ViewItemType = type;
        }

        protected void LoadInfo(ViewItemType type, int id, T parent)
        {
            Id = id;
            IsItemLoaded = true;
            ViewItemType = type;
            Parent = parent;
        }

        public void ToggleFilters() => ShowFilters = !ShowFilters;

        public int FilterCount()
        {
            var count = 0;
            // TODO count
            //if (Filters != null)
            //{
            //    Filters
            //    foreach (var filter in Filters)
            //    {
            //        switch (filter)
            //        {
            //            case SelectFilter<string> selectFilter:
            //                if (selectFilter.IsFiltered())
            //                {
            //                    count++;
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
            return count;
        }
    }
}
