using Emergence.Data.Shared;

namespace Emergence.Client
{
    public interface ISortable<T>
    {
        string SortBy { get; set; }
        SortDirection SortDirection { get; set; }
    }
}
