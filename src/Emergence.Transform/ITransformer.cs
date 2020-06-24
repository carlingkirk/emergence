namespace Emergence.Transform
{
    public interface ITransformer<T, in T1>
    {
        T Transform(T1 source);
    }
}
