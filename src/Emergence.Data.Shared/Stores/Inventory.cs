using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Inventory : IAuditable
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
