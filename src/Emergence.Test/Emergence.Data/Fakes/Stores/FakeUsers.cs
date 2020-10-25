using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeUsers
    {
        public static IEnumerable<User> Get() => new List<User>
            {
                new User
                {
                    Id = 1,
                    UserId = Guid.NewGuid(),
                    FirstName = "Daria",
                    LastName = "",
                    Photo = FakePhotos.Get().First(),
                    PhotoId = 1,
                    Location = FakeLocations.Get().First(),
                    EmailUpdates = true,
                    SocialUpdates = true,
                    DisplayName = "",
                    Bio = "",
                    ProfileVisibility = Visibility.Public,
                    ActivityVisibility = Visibility.Public,
                    OriginVisibility = Visibility.Public,
                    PlantInfoVisibility = Visibility.Public,
                    SpecimenVisibility = Visibility.Public,
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom(5) * -1),
                    DateModified = null
                }
            };
    }
}
