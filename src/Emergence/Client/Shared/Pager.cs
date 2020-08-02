using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Shared
{
    public partial class Pager<T> : ComponentBase
    {
        [Parameter]
        public int CurrentPage { get; set; }
        [Parameter]
        public int Count { get; set; }
        [Parameter]
        public int Take { get; set; }
        public int TotalPages { get; set; }
        [Parameter]
        public IEnumerable<T> Values { get; set; }
        [Parameter]
        public EventCallback<IEnumerable<T>> ValuesChanged { get; set; }
        [Parameter]
        public Func<int, Task<IEnumerable<T>>> Page { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Count > 0 && Take > 0)
            {
                TotalPages = (int)Math.Ceiling(Count / (double)Take);
            }

            await Task.CompletedTask;
        }

        protected async Task DoPage(int page)
        {
            CurrentPage += page;
            Values = await Page.Invoke(CurrentPage);
            await ValuesChanged.InvokeAsync(Values);
        }
    }
}
