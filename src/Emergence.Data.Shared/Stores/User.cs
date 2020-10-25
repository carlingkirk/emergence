using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class User
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(36)]
        public string DisplayName { get; set; }
        [StringLength(36)]
        public string FirstName { get; set; }
        [StringLength(36)]
        public string LastName { get; set; }
        [StringLength(500)]
        public string Bio { get; set; }
        public int? PhotoId { get; set; }
        public int? LocationId { get; set; }
        public bool EmailUpdates { get; set; }
        public bool SocialUpdates { get; set; }
        public Visibility ProfileVisibility { get; set; }
        public Visibility SpecimenVisibility { get; set; }
        public Visibility PlantInfoVisibility { get; set; }
        public Visibility OriginVisibility { get; set; }
        public Visibility ActivityVisibility { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Photo Photo { get; set; }
        public Location Location { get; set; }
    }
}
