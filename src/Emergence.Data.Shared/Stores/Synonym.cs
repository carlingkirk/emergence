using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Synonym : IIncludable<Synonym>, IIncludable<Synonym, Origin>, IIncludable<Synonym, Taxon>
    {
        public int Id { get; set; }
        public int TaxonId { get; set; }
        public Taxon Taxon { get; set; }
        public int? OriginId { get; set; }
        public Origin Origin { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(36)]
        public string Rank { get; set; }
        [StringLength(100)]
        public string Language { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
