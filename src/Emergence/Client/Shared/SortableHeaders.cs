using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Shared
{
    public partial class SortableHeaders<T> : ComponentBase, ISortable<T>
    {
        [Parameter]
        public Dictionary<string, string> HeaderNames { get; set; }
        [Parameter]
        public IEnumerable<T> Values { get; set; }
        [Parameter]
        public EventCallback<IEnumerable<T>> ValuesChanged { get; set; }
        [Parameter]
        public Func<string, SortDirection, Task<IEnumerable<T>>> Sort { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }

        protected string GetSortClass(string header)
        {
            if (SortBy == header)
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

        protected async Task DoSort(string header)
        {
            if (SortDirection != SortDirection.Ascending)
            {
                SortDirection = SortDirection.Ascending;
            }
            else
            {
                SortDirection = SortDirection.Descending;
            }

            SortBy = header;
            Values = await Sort.Invoke(header, SortDirection);
            await ValuesChanged.InvokeAsync(Values);
        }
    }
}
