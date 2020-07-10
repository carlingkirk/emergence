using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeActivities
    {
        public static IEnumerable<Activity> Get()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    ActivityId = 1,
                    ActivityType = ActivityType.Stratification,
                    Specimen = FakeSpecimens.Get().First(s => s.InventoryItem.Name == "Liatris spicata Seeds"),
                    Name = "Liatris spicata Seeds Stratification",
                    Description = "Stratify in fridge in woody mix",
                    DateScheduled = new System.DateTime(2020,03,25),
                    DateOccured = new System.DateTime(2020,03,25),
                    DateCreated = new System.DateTime(2020,07,10),
                    DateModified = null
                },
                new Activity
                {
                    ActivityId = 2,
                    ActivityType = ActivityType.Germination,
                    Specimen = FakeSpecimens.Get().First(s => s.InventoryItem.Name == "Liatris spicata Seeds"),
                    Name = "Liatris spicata Seeds Germination",
                    Description = "Put outside in greenhouse",
                    DateScheduled = new System.DateTime(2020,05,24),
                    DateOccured = new System.DateTime(2020,05,25),
                    DateCreated = new System.DateTime(2020,07,09),
                    DateModified = new System.DateTime(2020,07,10)
                },
                new Activity
                {
                    ActivityId = 2,
                    ActivityType = ActivityType.Division,
                    Specimen = FakeSpecimens.Get().First(s => s.InventoryItem.Name == "Liatris spicata Seeds"),
                    Name = "Liatris spicata Seeds Division",
                    Description = "Split about 15 seedlings into 2\" containers with woody mix",
                    DateScheduled = new System.DateTime(2020,06,24),
                    DateOccured = new System.DateTime(2020,06,12),
                    DateCreated = new System.DateTime(2020,07,10),
                    DateModified = null
                }
            };
            return activities;
        }
    }
}