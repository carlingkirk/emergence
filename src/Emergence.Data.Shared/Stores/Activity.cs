using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Activity : IIncludable<Activity>, IIncludable<Activity, Specimen>, IAuditable, IVisibile<Activity>
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [StringLength(36)]
        public string ActivityType { get; set; }
        [StringLength(100)]
        public string CustomActivityType { get; set; }
        public int? SpecimenId { get; set; }
        public int? Quantity { get; set; }
        [StringLength(36)]
        public string AssignedTo { get; set; }
        public DateTime? DateScheduled { get; set; }
        public DateTime? DateOccurred { get; set; }
        public Visibility Visibility { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Specimen Specimen { get; set; }
        public User User { get; set; }
    }
}
