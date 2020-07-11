using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeSpecimens
    {
        public static IEnumerable<Specimen> Get() => new List<Specimen>
        {
            new Specimen
            {
                Id = 1,
                InventoryItemId = 1,
                SpecimenStage = "Seed",
                InventoryItem = FakeInventories.GetItems().First(i => i.Id == 1)
            },
            new Specimen
            {
                Id = 2,
                InventoryItemId = 2,
                SpecimenStage = "Stratification"
            },
            new Specimen
            {
                Id = 3,
                InventoryItemId = 3,
                SpecimenStage = "Growing"
            }
        };
    }
}
