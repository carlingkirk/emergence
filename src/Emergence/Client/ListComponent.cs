using System;
using System.Collections.Generic;
using System.Linq;
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
        [Parameter]
        public bool LinkRelations { get; set; }
        [Parameter]
        public string ForUserId { get; set; }
        public IEnumerable<T> List { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        public int Take { get; set; }
        public int Count { get; set; }
        public bool ShowPublic { get; set; }
        public bool IsItemLoaded { get; set; }
        public bool ListView { get; set; }
        public int Id { get; set; }
        public T Parent { get; set; }
        public ViewItemType ViewItemType { get; set; }

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
            Take = 12;
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

        protected async Task<IEnumerable<T>> PageAsync(int page)
        {
            CurrentPage = page;
            await FindAsync();
            return List;
        }

        protected async Task<IEnumerable<T>> RefreshAsync()
        {
            await FindAsync();

            if (List.Count() == 0 && CurrentPage > 1)
            {
                CurrentPage--;
                await PageAsync(CurrentPage);
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

        protected string GetRandomColor(string search)
        {
            var colors = new Dictionary<string, string>
            {
                { "black", "#333333" },
                { "white", "#F9FFF7" },
                { "gray", "#ADAFA8" },
                { "red", "#E24441" },
                { "orange", "#E8AD2E" },
                { "yellow", "#EFEF6E" },
                { "green", "#94C973" },
                { "blue", "#5959FF" },
                { "purple", "#963AFF" },
                { "pink", "#FF77D6" },
                { "teal", "#8EFFD5" },
                { "olive","#3D550C" }
            };

            if (!string.IsNullOrEmpty(search))
            {
                foreach (var color in colors)
                {
                    if (search.Contains(color.Key))
                    {
                        return color.Value;
                    }
                }
            }

            var random = new Random();
            var colorPick = random.Next(1, colors.Count - 1);
            return colors.ElementAt(colorPick).Value;
        }
    }
}
