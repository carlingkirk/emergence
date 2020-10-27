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
                    UserId = Helpers.UserId,
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
                    InventoryItemVisibility = Visibility.Public,
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom(5) * -1),
                    DateModified = null
                }
            };

        public static User GetPrivateUser() => new User
        {
            Id = 2,
            UserId = Helpers.PrivateUserId,
            FirstName = "Jane",
            LastName = "Lane",
            Photo = FakePhotos.Get().First(),
            PhotoId = 1,
            Location = FakeLocations.Get().First(),
            EmailUpdates = true,
            SocialUpdates = true,
            DisplayName = "JaneLane",
            Bio = "Don't look at me",
            ProfileVisibility = Visibility.Hidden,
            ActivityVisibility = Visibility.Hidden,
            OriginVisibility = Visibility.Hidden,
            PlantInfoVisibility = Visibility.Hidden,
            InventoryItemVisibility = Visibility.Hidden,
            DateCreated = Helpers.Today.AddDays(Helpers.GetRandom(5) * -1),
            DateModified = null
        };
    }
}
