using System;
using Emergence.Data.Shared.Interfaces;

namespace Emergence.Data.Shared.Stores
{
    public class UserContactRequest : IContact, IIncludable<UserContactRequest>, IIncludable<UserContactRequest, User>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ContactUserId { get; set; }
        public DateTime DateRequested { get; set; }

        public User User { get; set; }
        public User ContactUser { get; set; }
    }
}
