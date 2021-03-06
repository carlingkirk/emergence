using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
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
        public IEnumerable<Visibility> Visibilities => Enum.GetValues(typeof(Visibility)).Cast<Visibility>();

        protected async Task Back() => await IsItemLoadedChanged.InvokeAsync(false);

        protected async Task UnloadItem()
        {
            await IsEditingChanged.InvokeAsync(false);
            await IsItemLoadedChanged.InvokeAsync(false);
        }

        protected async Task RefreshListAsync()
        {
            if (RefreshList != null)
            {
                List = await RefreshList.Invoke();
                await ListChanged.InvokeAsync(List);
            }
        }
    }
}
