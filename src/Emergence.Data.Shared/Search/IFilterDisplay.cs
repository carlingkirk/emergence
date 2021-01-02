namespace Emergence.Data.Shared.Search
{
    public interface IFilterDisplay<in TValue>
    {
        public string DisplayValue(TValue value);
    }
}
