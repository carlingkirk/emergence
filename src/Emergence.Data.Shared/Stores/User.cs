using System;

namespace Emergence.Data.Shared.Stores
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PhotoId { get; set; }
        public int? LocationId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Photo Photo { get; set; }
        public Location Location { get; set; }
    }
}
