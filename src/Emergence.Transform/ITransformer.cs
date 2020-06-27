using Emergence.Data.Shared.Stores;

namespace Emergence.Transform
{
    public interface ITransformer<T, in T1>
    {
        public Origin Origin { get; }
        T Transform(T1 source);
    }
}
