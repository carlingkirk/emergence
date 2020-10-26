using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared
{
    public interface IVisibile
    {
        public Visibility Visibility { get; set; }
        public User User { get; set; }
    }
    public interface IVisibile<T> : IVisibile
    {
    }
}
