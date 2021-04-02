using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Shared
{
    public partial class Pager<T> : ComponentBase, IPageable<T>
    {
        [Parameter]
        public int CurrentPage { get; set; }
        [Parameter]
        public long Count { get; set; }
        [Parameter]
        public int Take { get; set; }
        public int TotalPages => (int)Math.Ceiling(Count / (double)Take);
        [Parameter]
        public IEnumerable<T> Values { get; set; }
        [Parameter]
        public EventCallback<IEnumerable<T>> ValuesChanged { get; set; }
        [Parameter]
        public Func<int, int, Task<IEnumerable<T>>> Page { get; set; }

        protected async Task DoPage(int page, int perPage)
        {
            Take = perPage;
            CurrentPage += page;
            Values = await Page.Invoke(CurrentPage, Take);
            await ValuesChanged.InvokeAsync(Values);
        }

        protected async void OnTakeChanged(ChangeEventArgs eventArgs)
        {
            var take = int.Parse(eventArgs.Value.ToString()); // 100
            var pageReset = (CurrentPage - 1) * -1;

            await DoPage(pageReset, take);
        }
    }
}
