using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Emergence.Data.Shared.Enums;

namespace Emergence.Data.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
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
        [JsonConverter(typeof(EnumDisplayConverter<Visibility>))]
        public Visibility ProfileVisibility { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<Visibility>))]
        public Visibility InventoryItemVisibility { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<Visibility>))]
        public Visibility PlantInfoVisibility { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<Visibility>))]
        public Visibility OriginVisibility { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<Visibility>))]
        public Visibility ActivityVisibility { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Photo Photo { get; set; }
        public Location Location { get; set; }

        public IEnumerable<UserContact> Contacts { get; set; }
        public IEnumerable<UserContactRequest> ContactRequests { get; set; }
        public bool IsViewerContact { get; set; }
        public bool IsViewerContactRequested { get; set; }
    }
}
