using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeActivities
    {
        public static IEnumerable<Activity> Get()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Id = 1,
                    ActivityType = "Stratification",
                    SpecimenId = 1,
                    Specimen = FakeSpecimens.Get().Where(s => s.Id == 1).First(),
                    Name = "Liatris spicata Seeds Stratification",
                    Description = "Stratify in fridge in woody mix",
                    DateScheduled = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateOccured = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateCreated = Helpers.Today,
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
                },
                new Activity
                {
                    Id = 1,
                    ActivityType = "Germination",
                    SpecimenId = 1,
                    Specimen = FakeSpecimens.Get().Where(s => s.Id == 1).First(),
                    Name = "Germinate: Liatris spicata ",
                    Description = "Put outside in greenhouse",
                    DateScheduled = null,
                    DateOccured = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateCreated = Helpers.Today,
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
                },
                new Activity
                {
                    Id = 1,
                    ActivityType = "Division",
                    SpecimenId = 1,
                    Specimen = FakeSpecimens.Get().Where(s => s.Id == 1).First(),
                    Name = "Divide: Liatris spicata",
                    Description = "Split about 15 seedlings into 2\" containers with woody mix",
                    DateScheduled = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateOccured = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateCreated = Helpers.Today,
                    DateModified = null
                }
            };
            return activities;

        }
    }
}
