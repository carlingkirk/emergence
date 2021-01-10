namespace Emergence.Client.Common
{
    public interface IPageable<T>
    {
        int CurrentPage { get; set; }
        int Take { get; set; }
        long Count { get; set; }
    }
}
