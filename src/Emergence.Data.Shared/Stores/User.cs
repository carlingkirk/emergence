using System;

namespace Emergence.Data.Shared.Stores
{
    public class User
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PhotoId { get; set; }
        public int? LocationId { get; set; }
        public bool EmailUpdates { get; set; }
        public bool SocialUpdates { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Photo Photo { get; set; }
        public Location Location { get; set; }
    }
}
