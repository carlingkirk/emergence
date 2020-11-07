using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeUserContacts
    {
        public static IEnumerable<UserContact> GetContacts() => new List<UserContact>
        {
            new UserContact
            {
                Id = 1,
                UserId = 1,
                ContactUserId = 2,
                DateAccepted = Helpers.Today,
                DateRequested = Helpers.Today.AddDays(-1)
            },
            new UserContact
            {
                Id = 1,
                UserId = 1,
                ContactUserId = 3,
                DateAccepted = Helpers.Today.AddDays(-1),
                DateRequested = Helpers.Today.AddDays(-2)
            },
            new UserContact
            {
                Id = 1,
                UserId = 1,
                ContactUserId = 4,
                DateAccepted = Helpers.Today.AddDays(-3),
                DateRequested = Helpers.Today.AddDays(-4)
            }
        };

        public static IEnumerable<UserContactRequest> GetContactRequests() => new List<UserContactRequest>
        {
            new UserContactRequest
            {
                Id = 1,
                UserId = 1,
                ContactUserId = 5,
                DateRequested = Helpers.Today.AddDays(-1)
            },
            new UserContactRequest
            {
                Id = 1,
                UserId = 1,
                ContactUserId = 6,
                DateRequested = Helpers.Today.AddDays(-2)
            },
            new UserContactRequest
            {
                Id = 1,
                UserId = 1,
                ContactUserId = 7,
                DateRequested = Helpers.Today.AddDays(-3)
            }
        };
    }
}
