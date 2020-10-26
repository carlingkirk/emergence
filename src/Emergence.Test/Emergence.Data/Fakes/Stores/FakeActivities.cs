using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
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
                    Quantity = 3,
                    AssignedTo = Helpers.UserId,
                    Visibility = Visibility.Public,
                    ModifiedBy = Helpers.UserId,
                    CreatedBy = Helpers.UserId,
                    DateScheduled = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateOccurred = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateCreated = Helpers.Today,
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5),
                    User = FakeUsers.Get().First()
                },
                new Activity
                {
                    Id = 2,
                    ActivityType = "Germination",
                    SpecimenId = 1,
                    Specimen = FakeSpecimens.Get().Where(s => s.Id == 1).First(),
                    Name = "Germinate: Liatris spicata ",
                    Description = "Put outside in greenhouse",
                    Quantity = 2,
                    AssignedTo = Helpers.UserId,
                    Visibility = Visibility.Contacts,
                    ModifiedBy = Helpers.UserId,
                    CreatedBy = Helpers.UserId,
                    DateScheduled = null,
                    DateOccurred = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateCreated = Helpers.Today,
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5),
                    User = FakeUsers.Get().First()
                },
                new Activity
                {
                    Id = 3,
                    ActivityType = "Custom",
                    CustomActivityType = "Divide",
                    SpecimenId = 10,
                    Specimen = FakeSpecimens.Get().Where(s => s.Id == 1).First(),
                    Name = "Divide: Liatris spicata",
                    Description = "Split about 15 seedlings into 2\" containers with woody mix",
                    Quantity = 3,
                    AssignedTo = Helpers.UserId,
                    Visibility = Visibility.Inherit,
                    ModifiedBy = null,
                    CreatedBy = Helpers.UserId,
                    DateScheduled = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateOccurred = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateCreated = Helpers.Today,
                    DateModified = null,
                    User = FakeUsers.Get().First()
                }
                ,
                new Activity
                {
                    Id = 4,
                    ActivityType = "Custom",
                    CustomActivityType = "Divide",
                    SpecimenId = 10,
                    Specimen = FakeSpecimens.Get().Where(s => s.Id == 4).First(),
                    Name = "Divide: Liatris spicata",
                    Description = "Split about 15 seedlings into 2\" containers with woody mix",
                    Quantity = 3,
                    AssignedTo = FakeUsers.GetPrivateUser().UserId.ToString(),
                    Visibility = Visibility.Inherit,
                    ModifiedBy = null,
                    CreatedBy = FakeUsers.GetPrivateUser().UserId.ToString(),
                    DateScheduled = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateOccurred = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateCreated = Helpers.Today,
                    DateModified = null,
                    User = FakeUsers.GetPrivateUser()
                }
            };
            return activities;

        }
    }
}
