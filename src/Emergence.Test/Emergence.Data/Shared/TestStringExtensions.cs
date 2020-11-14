using System.Collections.Generic;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared
{
    public class TestStringExtensions
    {
        [Theory]
        [MemberData(nameof(GetZones))]
        public void TestParseZone(string zone, Zone result)
        {
            var zoneResult = zone.ParseZone();
            zoneResult?.Number.Should().Be(result?.Number);
            zoneResult?.Letter.Should().Be(result?.Letter);
        }

        public static IEnumerable<object[]> GetZones()
        {
            yield return new object[] { "9", new Zone { Number = 9 } };
            yield return new object[] { "9a", new Zone { Number = 9, Letter = "a" } };
            yield return new object[] { "9B", new Zone { Number = 9, Letter = "b" } };
            yield return new object[] { "10", new Zone { Number = 10 } };
            yield return new object[] { "10a", new Zone { Number = 10, Letter = "a" } };
            yield return new object[] { "10B", new Zone { Number = 10, Letter = "b" } };
            yield return new object[] { "0", new Zone { Number = 0 } };
            yield return new object[] { "b", null };
            yield return new object[] { "%", null };
        }
    }
}
