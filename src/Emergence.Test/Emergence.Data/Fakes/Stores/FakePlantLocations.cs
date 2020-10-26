using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakePlantLocations
    {
        public static IEnumerable<PlantLocation> Get()
        {
            var plantLocations = new List<PlantLocation>
            {
                new PlantLocation
                {
                    Id = 1,
                    PlantInfoId = 1,
                    LocationId = 1,
                    Status = "Native",
                    CreatedBy = Helpers.UserId,
                    DateCreated = Helpers.Today.AddMonths(-1).AddDays(5)
                }
            };
            return plantLocations;
        }
    }
}
