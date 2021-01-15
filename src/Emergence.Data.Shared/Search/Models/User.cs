using System;
using System.Collections.Generic;

namespace Emergence.Data.Shared.Search.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public int? PhotoId { get; set; }
        public int? LocationId { get; set; }
        public bool EmailUpdates { get; set; }
        public bool SocialUpdates { get; set; }
        public Visibility ProfileVisibility { get; set; }
        public Visibility InventoryItemVisibility { get; set; }
        public Visibility PlantInfoVisibility { get; set; }
        public Visibility OriginVisibility { get; set; }
        public Visibility ActivityVisibility { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Photo Photo { get; set; }
        public Location Location { get; set; }
        public IEnumerable<int> ContactIds { get; set; }
    }
}
