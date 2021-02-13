using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeSpecimens
    {
        public static IEnumerable<Specimen> Get()
        {
            var today = Helpers.Today;
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
                    DateCreated = today,
                    DateModified = today.AddHours(1).AddMinutes(5),
                    Notes = "Liatris spicata, the dense blazing star or prairie gay feather, is an herbaceous perennial flowering plant in the sunflower and daisy family " +
                    "Asteraceae. It is native to eastern North America where it grows in moist prairies and sedge meadows. The plants have tall spikes of purple flowers " +
                    "resembling bottle brushes or feathers that grow one to five feet tall. The species grows in hardiness zones 3 - 8, stretching from the Midwest " +
                    "to the East Coast, eastern and western Canada.",
                    CreatedBy = Helpers.UserId,
                    ModifiedBy = null,
                    ParentSpecimen = null
                },
                new Specimen
                {
                    Id = 2,
                    InventoryItemId = 2,
                    SpecimenStage = "Stratification",
                    LifeformId = 2,
                    InventoryItem = FakeInventories.GetItems().First(i => i.Id == 2),
                    Lifeform = FakeLifeforms.Get().First(l => l.Id == 2),
                    DateCreated = today,
                    DateModified = today.AddHours(1).AddMinutes(5)
                },
                new Specimen
                {
                    Id = 3,
                    InventoryItemId = 3,
                    SpecimenStage = "Growing",
                    LifeformId = 3,
                    InventoryItem = FakeInventories.GetItems().First(i => i.Id == 3),
                    Lifeform = FakeLifeforms.Get().First(l => l.Id == 3),
                    DateCreated = today,
                    DateModified = today.AddHours(1).AddMinutes(5)
                },
                new Specimen
                {
                    Id = 4,
                    InventoryItemId = 4,
                    SpecimenStage = "Seed",
                    LifeformId = 4,
                    InventoryItem = FakeInventories.GetItems().First(i => i.Id == 4),
                    Lifeform = FakeLifeforms.Get().First(l => l.Id == 4),
                    DateCreated = today,
                    DateModified = today.AddHours(1).AddMinutes(5)
                }
            };

            return specimens;
        }
    }
}
