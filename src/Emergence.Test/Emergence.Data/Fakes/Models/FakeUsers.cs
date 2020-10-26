using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeUsers
    {
        public static IEnumerable<User> Get() => new List<User>
            {
                new User
                {
                    Id = 1,
                    UserId = new Guid(Helpers.UserId),
                    FirstName = "Daria",
                    LastName = "Morgendorffer",
                    Photo = FakePhotos.Get().First(),
                    Location = FakeLocations.Get().First(),
                    EmailUpdates = true,
                    SocialUpdates = true,
                    DisplayName = "",
                    Bio = "",
                    ProfileVisibility = Visibility.Public,
                    ActivityVisibility = Visibility.Public,
                    OriginVisibility = Visibility.Public,
                    PlantInfoVisibility = Visibility.Public,
                    InventoryItemVisibility = Visibility.Public,
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom(5) * -1),
                    DateModified = null
                }
            };
    }
}
