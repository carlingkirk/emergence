using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeSpecimens
    {
        public static IEnumerable<Specimen> Get() => new List<Specimen>
        {
            new Specimen
            {
                Id = 0,
                InventoryItemId = 0,
                SpecimenStage = "Seed"
            },
            new Specimen
            {
                Id = 1,
                InventoryItemId = 1,
                SpecimenStage = "Stratification"
            },
            new Specimen
            {
                Id = 2,
                InventoryItemId = 2,
                SpecimenStage = "Growing"
            }
        };
    }
}
