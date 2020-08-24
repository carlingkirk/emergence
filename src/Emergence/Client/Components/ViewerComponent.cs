using Emergence.Client.Common;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ViewerComponent : EmergenceComponent
    {
        [Parameter]
        public int Id { get; set; }
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
    }
}