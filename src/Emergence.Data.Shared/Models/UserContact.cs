using System;

namespace Emergence.Data.Shared.Models
{
    public class UserContact
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContactUserId { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime DateAccepted { get; set; }
    }
}
