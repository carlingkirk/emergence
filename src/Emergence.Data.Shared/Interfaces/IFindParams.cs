namespace Emergence.Data.Shared.Interfaces
{
    public interface IFindParams
    {
        string SearchText { get; set; }
        int Skip { get; set; }
        int Take { get; set; }
        string SortBy { get; set; }
        SortDirection SortDirection { get; set; }
    }
}
