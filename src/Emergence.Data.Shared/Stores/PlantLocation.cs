using System;

namespace Emergence.Data.Shared.Stores
{
    public class PlantLocation : IAuditable
    {
        public int Id { get; set; }
        public int PlantInfoId { get; set; }
        public int LocationId { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public PlantInfo PlantInfo { get; set; }
        public Location Location { get; set; }
    }
}
