using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Emergence.Data.Repository;
using Emergence.Data.Shared.Stores;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using Xunit;

namespace Emergence.Test.Emergence.Data
{
    public class RepositoryTests
    {
        [Fact]
        public async Task TestGetSomeAsync()
        {
            var mockSet = new Mock<DbSet<Specimen>>();
            var specimens = FakeSpecimens.Get().AsQueryable();

            mockSet.As<IQueryable<Specimen>>().Setup(m => m.Expression).Returns(specimens.Expression);
            mockSet.As<IQueryable<Specimen>>().Setup(m => m.ElementType).Returns(specimens.ElementType);
            mockSet.As<IQueryable<Specimen>>().Setup(m => m.GetEnumerator()).Returns(specimens.GetEnumerator());
            mockSet.As<IDbAsyncEnumerable<Specimen>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Specimen>(specimens.GetEnumerator()));
            mockSet.As<IQueryable<Specimen>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Specimen>(specimens.Provider));
            //mockSet.As<IQueryable<Specimen>>()
            //    .Setup(m => m.Provider)
            //    .Returns(new TestAsyncQueryProvider<Specimen>(specimens.Provider));

            var mockContext = new Mock<EmergenceDbContext>();
            mockContext.Setup(c => c.Set<Specimen>()).Returns(mockSet.Object);

            var specimenRepository = new Repository<Specimen>(mockContext.Object);
            var specimen = await specimenRepository.GetAsync(s => s.Id == 1);

            specimen.InventoryItemId.Should().Be(1);
            specimen.SpecimenStage.Should().Be("Seed");
            specimen.InventoryItem.Should().NotBeNull();
        }
    }

    internal class TestDbAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression) => new TestDbAsyncEnumerable<TEntity>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestDbAsyncEnumerable<TElement>(expression);

        //public object Execute(Expression expression) => _inner.Execute(expression);
        public object Execute(Expression expression) => CompileExpressionItem<object>(expression);
        public TResult Execute<TResult>(Expression expression) => CompileExpressionItem<TResult>(expression);
        //public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken) => Task.FromResult(Execute(expression));

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => Task.FromResult(Execute<TResult>(expression));

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => Execute<TResult>(expression);

        private static TResult CompileExpressionItem<TResult>(Expression expression)
        {
            if (expression.Type.IsGenericType)
            {

            }
            else
            {

            }
            var visitor = new TestExpressionVisitor();
            var body = visitor.Visit(expression);
            var f = Expression.Lambda<Func<TResult>>(body ?? throw new InvalidOperationException($"{nameof(body)} is null"), (IEnumerable<ParameterExpression>)null);
            return f.Compile()();
        }
    }

    public class TestExpressionVisitor : ExpressionVisitor
    {
    }

    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression) => new TestDbAsyncEnumerable<TEntity>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestDbAsyncEnumerable<TElement>(expression);

        public object Execute(Expression expression) => _inner.Execute(expression);

        public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default) => Execute<TResult>(expression);
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator() => new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => GetAsyncEnumerator();

        IQueryProvider IQueryable.Provider => new TestDbAsyncQueryProvider<T>(this);
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose() => _inner.Dispose();

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken) => Task.FromResult(_inner.MoveNext());

        public T Current => _inner.Current;

        object IDbAsyncEnumerator.Current => Current;
    }
}
