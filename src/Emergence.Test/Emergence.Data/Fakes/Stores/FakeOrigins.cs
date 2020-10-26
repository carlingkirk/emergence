using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeOrigins
    {
        public static IEnumerable<Origin> Get() => new List<Origin>
        {
            new Origin
            {
                Id = 1,
                Name = "Plant Finder - Missouri Botanical Garden",
                Description = "Look up, view a photo and read about the over 7,500 plants which are growing or have been grown in the Kemper Center display " +
                "gardens (plus selected additions) by scientific name, common name and/or selected plant characteristics.",
                Type = "Website",
                Uri = new Uri("http://www.missouribotanicalgarden.org/plantfinder/plantfindersearch.aspx"),
                Authors = "fred",
                AltExternalId = "44",
                ExternalId = "222",
                Location = FakeLocations.Get().First(),
                Visibility = Visibility.Contacts,
                CreatedBy = FakeUsers.Get().First().UserId.ToString(),
                ModifiedBy = FakeUsers.Get().First().UserId.ToString(),
                DateCreated = Helpers.Today.AddMonths(-1).AddDays(-5),
                DateModified = Helpers.Today.AddMonths(-1).AddDays(5),
                User = FakeUsers.Get().First()
            },
            new Origin
            {
                Id = 2,
                ParentOriginId = 3,
                Type = "Nursery",
                Name = "Botany Yards",
                Description = "",
                Authors = "",
                AltExternalId = "444",
                ExternalId = "srtyhs",
                Location = FakeLocations.Get().First(),
                Visibility = Visibility.Public,
                CreatedBy = FakeUsers.Get().First().UserId.ToString(),
                ModifiedBy = null,
                DateCreated = Helpers.Today.AddMonths(-1).AddDays(5),
                DateModified = null,
                User = FakeUsers.Get().First()
            },
            new Origin
            {
                Id = 3,
                Type = "Event",
                Name = "GNPS Symposium 2020",
                Description = "Our 25th annual Symposium is all about connections between native plants and the fauna that rely upon them. Inspired by the " +
                "February book release of Nature’s Best Hope: A New Approach to Conservation that Starts in Your Yard from Doug Tallamy, we invite you to " +
                "be among the first to hear his new message. Growing more native plants to sustain all of nature is more urgent than ever.",
                Uri = new Uri("https://gnps.org/2020-georgia-native-plant-society-annual-symposium/"),
                Authors = "linda",
                AltExternalId = "#$%#^",
                ExternalId = "GGGGGGGGG",
                Location = FakeLocations.Get().First(),
                Visibility = Visibility.Hidden,
                CreatedBy = FakeUsers.Get().First().UserId.ToString(),
                ModifiedBy = FakeUsers.Get().First().UserId.ToString(),
                DateCreated = Helpers.Today.AddMonths(-1).AddDays(-5),
                DateModified = Helpers.Today.AddMonths(-1).AddDays(5),
                User = FakeUsers.Get().First()
            },
            new Origin
            {
                Id = 4,
                ParentOriginId = 3,
                Type = "Nursery",
                Name = "Botany Yards",
                Description = "",
                Authors = "",
                AltExternalId = "444",
                ExternalId = "srtyhs",
                Location = FakeLocations.Get().First(),
                Visibility = Visibility.Inherit,
                CreatedBy = FakeUsers.GetPrivateUser().UserId.ToString(),
                ModifiedBy = null,
                DateCreated = Helpers.Today.AddMonths(-1).AddDays(5),
                DateModified = null,
                User = FakeUsers.GetPrivateUser()
            }
        };
    }
}
