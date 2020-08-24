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
                LifeformId = 1,
                InventoryItem = FakeInventories.GetItems().First(i => i.InventoryId == 1),
                Lifeform = FakeLifeforms.Get().First(l => l.Id == 1),
                DateCreated = Helpers.Today,
                DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
            },
            new Specimen
            {
                Id = 2,
                InventoryItemId = 1,
                SpecimenStage = "Stratification",
                LifeformId = 1,
                InventoryItem = FakeInventories.GetItems().First(i => i.InventoryId == 1),
                Lifeform = FakeLifeforms.Get().First(l => l.Id == 1),
                DateCreated = Helpers.Today,
                DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
            },
            new Specimen
            {
                Id = 3,
                InventoryItemId = 1,
                SpecimenStage = "Growing",
                LifeformId = 1,
                InventoryItem = FakeInventories.GetItems().First(i => i.InventoryId == 1),
                Lifeform = FakeLifeforms.Get().First(l => l.Id == 1),
                DateCreated = Helpers.Today,
                DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
            }
        };
    }
}
