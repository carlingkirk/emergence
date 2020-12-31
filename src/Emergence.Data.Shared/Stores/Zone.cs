using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Zone
    {
        public int Id { get; set; }
        [StringLength(3)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
    }
}
