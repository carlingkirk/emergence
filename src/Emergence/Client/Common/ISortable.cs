using Emergence.Data.Shared;

namespace Emergence.Client.Common
{
    public interface ISortable<T>
    {
        string SortBy { get; set; }
        SortDirection SortDirection { get; set; }
    }
}
