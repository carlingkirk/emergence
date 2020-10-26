using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeSpecimens
    {
        public static IEnumerable<Specimen> Get()
        {
            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    Id = 1,
                    InventoryItemId = 1,
                    SpecimenStage = "Seed",
                    LifeformId = 1,
                    InventoryItem = FakeInventories.GetItems().First(i => i.Id == 1),
                    Lifeform = FakeLifeforms.Get().First(l => l.Id == 1),
                    DateCreated = Helpers.Today,
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
                },
                new Specimen
                {
                    Id = 2,
                    InventoryItemId = 2,
                    SpecimenStage = "Stratification",
                    LifeformId = 2,
                    InventoryItem = FakeInventories.GetItems().First(i => i.Id == 2),
                    Lifeform = FakeLifeforms.Get().First(l => l.Id == 2),
                    DateCreated = Helpers.Today,
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
                },
                new Specimen
                {
                    Id = 3,
                    InventoryItemId = 3,
                    SpecimenStage = "Growing",
                    LifeformId = 3,
                    InventoryItem = FakeInventories.GetItems().First(i => i.Id == 3),
                    Lifeform = FakeLifeforms.Get().First(l => l.Id == 3),
                    DateCreated = Helpers.Today,
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
                },
                new Specimen
                {
                    Id = 4,
                    InventoryItemId = 4,
                    SpecimenStage = "Seed",
                    LifeformId = 4,
                    InventoryItem = FakeInventories.GetItems().First(i => i.Id == 4),
                    Lifeform = FakeLifeforms.Get().First(l => l.Id == 4),
                    DateCreated = Helpers.Today,
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
                }
            };

            return specimens;
        }
    }
}
