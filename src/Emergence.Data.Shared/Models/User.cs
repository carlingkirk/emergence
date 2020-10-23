using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        [Required]
        [RegularExpression("\\w")]
        public string DisplayName { get; set; }
        [MaxLength(36)]
        public string FirstName { get; set; }
        [MaxLength(36)]
        public string LastName { get; set; }
        [MaxLength(500)]
        public string Bio { get; set; }
        public bool EmailUpdates { get; set; }
        public bool SocialUpdates { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Photo Photo { get; set; }
        public Location Location { get; set; }
    }
}
