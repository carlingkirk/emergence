namespace Emergence.Client
{
    public interface IPageable<T>
    {
        int CurrentPage { get; set; }
        int Take { get; set; }
        int Count { get; set; }
    }
}
