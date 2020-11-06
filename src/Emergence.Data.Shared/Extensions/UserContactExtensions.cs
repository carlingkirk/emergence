using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class UserContactExtensions
    {
        public static Models.UserContact AsModel(this UserContact source) => new Models.UserContact
        {
            Id = source.Id,
            UserId = source.UserId,
            ContactUserId = source.ContactUserId.Value,
            DateAccepted = source.DateAccepted,
            DateRequested = source.DateRequested
        };

        public static UserContact AsStore(this Models.UserContact source) => new UserContact
        {
            Id = source.Id,
            UserId = source.UserId,
            ContactUserId = source.ContactUserId,
            DateAccepted = source.DateAccepted,
            DateRequested = source.DateRequested
        };
    }
}
