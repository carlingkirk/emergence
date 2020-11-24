using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Common
{
    public interface ISearchable<T>
    {
        [Parameter]
        bool ShowSearch { get; set; }
        string SearchText { get; set; }
        Task SearchAsync();
    }
}
