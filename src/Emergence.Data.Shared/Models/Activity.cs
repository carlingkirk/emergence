using System;
using System.Collections.Generic;

namespace Emergence.Data.Shared.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public string CustomActivityType { get; set; }
        public int? Quantity { get; set; }

        public string AssignedTo { get; set; }
        public DateTime? DateOccured { get; set; }
        public DateTime? DateScheduled { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Specimen Specimen { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }
}
