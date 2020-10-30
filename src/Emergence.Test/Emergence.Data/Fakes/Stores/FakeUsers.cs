using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeUsers
    {
        public static IEnumerable<User> Get() => new List<User>
        {
            GetPublic(),
            GetContact(),
            GetPrivate()
        };

        public static User GetPublic() => new User
        {
            Id = 1,
            UserId = Helpers.UserId,
            FirstName = "Daria",
            LastName = "",
            Photo = FakePhotos.Get().First().AsModel("https://blobs.com/photos/").AsStore(),
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
            DateModified = null,
            Contacts = new List<UserContact>
            {
                new UserContact
                {
                    Id = 1,
                    UserId = 1,
                    ContactUserId = 2,
                    DateRequested = Helpers.Today,
                    DateAccepted = Helpers.Today
                }
            }
        };

        public static User GetPrivate() => new User
        {
            Id = 2,
            UserId = Helpers.PrivateUserId,
            FirstName = "Jane",
            LastName = "Lane",
            Photo = FakePhotos.Get().First().AsModel("https://blobs.com/photos/").AsStore(),
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

        public static User GetContact() => new User
        {
            Id = 3,
            UserId = Helpers.PrivateUserId,
            FirstName = "Mr.",
            LastName = "Belvedere",
            Photo = FakePhotos.Get().First().AsModel("https://blobs.com/photos/").AsStore(),
            PhotoId = 1,
            Location = FakeLocations.Get().First(),
            EmailUpdates = true,
            SocialUpdates = true,
            DisplayName = "Belvedere",
            Bio = "Hey friend",
            ProfileVisibility = Visibility.Contacts,
            ActivityVisibility = Visibility.Contacts,
            OriginVisibility = Visibility.Contacts,
            PlantInfoVisibility = Visibility.Contacts,
            InventoryItemVisibility = Visibility.Contacts,
            DateCreated = Helpers.Today.AddDays(Helpers.GetRandom(5) * -1),
            DateModified = null,
            Contacts = new List<UserContact>
            {
                new UserContact
                {
                    Id = 1,
                    UserId = 3,
                    ContactUserId = 1,
                    DateRequested = Helpers.Today,
                    DateAccepted = Helpers.Today
                }
            }
        };
    }
}
