using System;

namespace Emergence.Data.Shared.Models
{
    public class Synonym
    {
        public int SynonymId { get; set; }
        public int TaxonId { get; set; }
        public Taxon Taxon { get; set; }
        public Origin Origin { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Language { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
