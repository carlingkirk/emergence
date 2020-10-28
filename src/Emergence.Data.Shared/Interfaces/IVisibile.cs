using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared
{
    public interface IVisibile
    {
        public Visibility Visibility { get; }
        public User User { get; }
    }
    public interface IVisibile<T> : IVisibile
    {
    }
}
