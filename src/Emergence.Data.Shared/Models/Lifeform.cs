using System;

namespace Emergence.Data.Shared.Models
{
    public class Lifeform
    {
        public int LifeformId { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

    }
}
