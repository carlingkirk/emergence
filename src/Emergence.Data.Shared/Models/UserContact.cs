using System;
using Emergence.Data.Shared.Interfaces;

namespace Emergence.Data.Shared.Models
{
    public class UserContact : IContact
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ContactUserId { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime DateAccepted { get; set; }

        public UserSummary User { get; set; }
        public UserSummary ContactUser { get; set; }
    }
}
