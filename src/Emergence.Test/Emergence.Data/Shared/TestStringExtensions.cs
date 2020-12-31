using Emergence.Data.Shared.Extensions;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared
{
    public class TestStringExtensions
    {
        [Fact]
        public void TestNullIfEmpty()
        {
            var value = "";
            value.NullIfEmpty().Should().Be(null);

            value = "haha";
            value.NullIfEmpty().Should().NotBe(null);
        }
    }
}
