using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Taxon : IAuditable
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Kingdom { get; set; }
        [StringLength(100)]
        public string Subkingdom { get; set; }
        [StringLength(100)]
        public string Infrakingdom { get; set; }
        [StringLength(100)]
        public string Phylum { get; set; }
        [StringLength(100)]
        public string Subphylum { get; set; }
        [StringLength(100)]
        public string Superclass { get; set; }
        [StringLength(100)]
        public string Class { get; set; }
        [StringLength(100)]
        public string Subclass { get; set; }
        [StringLength(100)]
        public string Infraclass { get; set; }
        [StringLength(100)]
        public string Superorder { get; set; }
        [StringLength(100)]
        public string Order { get; set; }
        [StringLength(100)]
        public string Suborder { get; set; }
        [StringLength(100)]
        public string Infraorder { get; set; }
        [StringLength(100)]
        public string Epifamily { get; set; }
        [StringLength(100)]
        public string Superfamily { get; set; }
        [StringLength(100)]
        public string Family { get; set; }
        [StringLength(100)]
        public string Subfamily { get; set; }
        [StringLength(100)]
        public string Supertribe { get; set; }
        [StringLength(100)]
        public string Tribe { get; set; }
        [StringLength(100)]
        public string Subtribe { get; set; }
        [StringLength(100)]
        public string GenusHybrid { get; set; }
        [StringLength(100)]
        public string Genus { get; set; }
        [StringLength(100)]
        public string Section { get; set; }
        [StringLength(100)]
        public string Subgenus { get; set; }
        [StringLength(100)]
        public string Hybrid { get; set; }
        [StringLength(100)]
        public string Species { get; set; }
        [StringLength(100)]
        public string Subspecies { get; set; }
        [StringLength(100)]
        public string Variety { get; set; }
        [StringLength(100)]
        public string Subvariety { get; set; }
        [StringLength(100)]
        public string Form { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
