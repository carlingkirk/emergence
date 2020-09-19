using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class PlantLocation : IAuditable
    {
        public int Id { get; set; }
        public int PlantInfoId { get; set; }
        public int LocationId { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public PlantInfo PlantInfo { get; set; }
        public Location Location { get; set; }
    }
}
