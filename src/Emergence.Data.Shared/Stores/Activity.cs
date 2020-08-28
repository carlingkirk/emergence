using System;

namespace Emergence.Data.Shared.Stores
{
    public class Activity : IIncludable<Activity>, IIncludable<Activity, Specimen>, IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActivityType { get; set; }
        public string CustomActivityType { get; set; }
        public int? SpecimenId { get; set; }
        public int Quantity { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? DateScheduled { get; set; }
        public DateTime? DateOccured { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Specimen Specimen { get; set; }
    }
}
