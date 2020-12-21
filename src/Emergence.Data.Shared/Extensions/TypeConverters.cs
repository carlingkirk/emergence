using System;
using System.Linq.Expressions;

namespace Emergence.Data.Shared.Extensions
{
    public sealed class BoxingSafeConverter<TIn, TOut>
    {
        public static readonly BoxingSafeConverter<TIn, TOut> Instance = new BoxingSafeConverter<TIn, TOut>();

        public Func<TIn, TOut> Convert { get; }

        private BoxingSafeConverter()
        {
            if (typeof(TIn) != typeof(TOut))
            {
                throw new InvalidOperationException("Both generic type parameters must represent the same type.");
            }
            var paramExpr = Expression.Parameter(typeof(TIn));
            Convert = Expression.Lambda<Func<TIn, TOut>>(paramExpr, paramExpr).Compile();
        }
    }
}
