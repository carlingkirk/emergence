using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
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
        public Visibility InventoryItemVisibility { get; set; }
        public Visibility PlantInfoVisibility { get; set; }
        public Visibility OriginVisibility { get; set; }
        public Visibility ActivityVisibility { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Photo Photo { get; set; }
        public Location Location { get; set; }

        public IEnumerable<UserContact> OthersContacts { get; set; }
        public IEnumerable<UserContact> Contacts { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public IEnumerable<InventoryItem> InventoryItems { get; set; }
        public IEnumerable<Origin> Origins { get; set; }
        public IEnumerable<PlantInfo> PlantInfos { get; set; }
    }
}
