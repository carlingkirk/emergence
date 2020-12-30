namespace Emergence.Data.Shared.Search
{
    public interface IEnumDisplay<in TValue>
    {
        public string DisplayValue(TValue value);
    }
}
