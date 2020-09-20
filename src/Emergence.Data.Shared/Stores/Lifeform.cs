using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Lifeform : IAuditable
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string ScientificName { get; set; }
        [StringLength(100)]
        public string CommonName { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
