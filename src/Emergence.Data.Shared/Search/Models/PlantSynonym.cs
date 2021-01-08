using System;

namespace Emergence.Data.Shared.Search.Models
{
    public class Synonym
    {
        public int Id { get; set; }
        public Origin Origin { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Language { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
