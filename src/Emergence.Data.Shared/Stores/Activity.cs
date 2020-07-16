using System;

namespace Emergence.Data.Shared.Stores
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActivityType { get; set; }
        public int? SpecimenId { get; set; }
        public Specimen Specimen { get; set; }
        public string UserId { get; set; }
        public DateTime? DateScheduled { get; set; }
        public DateTime? DateOccured { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
