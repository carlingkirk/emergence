using System;
using System.Text.Json.Serialization;
using Emergence.Data.Shared.Enums;

namespace Emergence.Data.Shared.Models
{
    public class PlantLocation
    {
        public int Id { get; set; }
        public PlantInfo PlantInfo { get; set; }
        public Location Location { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<LocationStatus>))]
        public LocationStatus Status { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<ConservationStatus>))]
        public ConservationStatus ConservationStatus { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
