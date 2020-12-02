namespace Emergence.Client.Common
{
    public interface IFilterable<T>
    {
        public bool ShowFilters { get; set; }
        public void ToggleFilters();
    }
}
