using System;

namespace Emergence.Data.Shared
{
    public interface IAuditable
    {
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
