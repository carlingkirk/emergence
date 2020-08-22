using System.Linq;
using Emergence.Data.Shared.Models;
using Emergence.Test.Data.Fakes.Models;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Data.Shared.Models
{

    public class TestLocation
    {
        [Fact]
        public void TestLocationDisplay()
        {
            var location = FakeLocations.Get().First();

            location.HasAddressInfo.Should().Be(true, "Location has address info");
            location.CityState.Should().Be("Macon, GA");
            location.LatLong.Should().Be("32.8087681, -83.7358335");

            location = new Location();
            location.HasAddressInfo.Should().Be(false, "Location has no address info");
            location.CityState.Should().Be("");
            location.LatLong.Should().Be("");

            location.City = "Macon";
            location.CityState.Should().Be("Macon");
            location.HasAddressInfo.Should().Be(true, "Location has address info");

            location.City = null;
            location.StateOrProvince = "GA";
            location.CityState.Should().Be("GA");
            location.HasAddressInfo.Should().Be(true, "Location has address info");
        }
    }
}
