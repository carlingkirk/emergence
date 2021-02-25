using System;

namespace Emergence.Data.Shared.Search.Models
{
    public class PlantLocation
    {
        public int Id { get; set; }
        public LocationStatus Status { get; set; }
        public ConservationStatus ConservationStatus { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Location Location { get; set; }
    }
}
