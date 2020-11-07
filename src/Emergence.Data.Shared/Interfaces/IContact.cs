using System;

namespace Emergence.Data.Shared.Interfaces
{
    public interface IContact
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ContactUserId { get; set; }
        public DateTime DateRequested { get; set; }
    }
}
