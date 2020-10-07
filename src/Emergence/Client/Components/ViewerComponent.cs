using Emergence.Client.Common;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ViewerComponent : EmergenceComponent
    {
        [Inject]
        public ListState ListState { get; set; }
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
        public EventCallback ItemLoadedChanged { get; set; }
        public bool IsEditable { get; set; }
        [Parameter]
        public bool IsEditing { get; set; }
        [Parameter]
        public EventCallback<bool> IsEditingChanged { get; set; }
    }
}
