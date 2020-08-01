using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Shared
{
    public partial class SortableHeader<T> : ComponentBase
    {
        [Parameter]
        public string HeaderName { get; set; }
        [Parameter]
        public IEnumerable<T> Values { get; set; }
        [Parameter]
        public EventCallback<IEnumerable<T>> ValuesChanged { get; set; }
        [CascadingParameter]
        public string SortBy { get; set; }
        [CascadingParameter]
        public SortDirection SortDirection { get; set; }
        [Parameter]
        public Func<string, SortDirection, Task<IEnumerable<T>>> Sort { get; set; }

        protected string GetSortClass()
        {
            if (SortBy == HeaderName)
            {
                if (SortDirection == SortDirection.Descending)
                {
                    return "oi oi-caret-bottom";
                }
                else
                {
                    return "oi oi-caret-top";
                }
            }
            return "";
        }

        protected async Task DoSort()
        {
            if (SortDirection == SortDirection.Descending)
            {
                SortDirection = SortDirection.Ascending;
            }
            else
            {
                SortDirection = SortDirection.Descending;
            }

            SortBy = HeaderName;
            Values = await Sort.Invoke(HeaderName, SortDirection);
            await ValuesChanged.InvokeAsync(Values);
        }
    }
}
