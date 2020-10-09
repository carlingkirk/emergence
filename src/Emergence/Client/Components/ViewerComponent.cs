using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ViewerComponent<T> : EmergenceComponent
    {
        [Parameter]
        public int Id { get; set; }
        [Parameter]
        public bool IsModal { get; set; }
        [Parameter]
        public EventCallback<int> IdChanged { get; set; }
        [Parameter]
        public bool IsItemLoaded { get; set; }
        [Parameter]
        public EventCallback<bool> IsItemLoadedChanged { get; set; }
        public bool IsEditable { get; set; }
        [Parameter]
        public bool IsEditing { get; set; }
        [Parameter]
        public EventCallback<bool> IsEditingChanged { get; set; }
        [Parameter]
        public IEnumerable<T> List { get; set; }
        [Parameter]
        public EventCallback<IEnumerable<T>> ListChanged { get; set; }
        [Parameter]
        public Func<Task<IEnumerable<T>>> RefreshList { get; set; }

        protected async Task Back() => await IsItemLoadedChanged.InvokeAsync(false);

        protected async Task UnloadItem()
        {
            if (RefreshList != null)
            {
                List = await RefreshList.Invoke();
                await ListChanged.InvokeAsync(List);
            }

            await IsEditingChanged.InvokeAsync(false);
            await IsItemLoadedChanged.InvokeAsync(false);
        }
    }
}
