using System;

namespace Emergence.Data.Shared.Search.Models
{
    public class Taxon
    {
        public int Id { get; set; }
        public string Kingdom { get; set; }
        public string Subkingdom { get; set; }
        public string Infrakingdom { get; set; }
        public string Phylum { get; set; }
        public string Subphylum { get; set; }
        public string Superclass { get; set; }
        public string Class { get; set; }
        public string Subclass { get; set; }
        public string Infraclass { get; set; }
        public string Superorder { get; set; }
        public string Order { get; set; }
        public string Suborder { get; set; }
        public string Infraorder { get; set; }
        public string Epifamily { get; set; }
        public string Superfamily { get; set; }
        public string Family { get; set; }
        public string Subfamily { get; set; }
        public string Supertribe { get; set; }
        public string Tribe { get; set; }
        public string Subtribe { get; set; }
        public string GenusHybrid { get; set; }
        public string Genus { get; set; }
        public string Section { get; set; }
        public string Subgenus { get; set; }
        public string Hybrid { get; set; }
        public string Species { get; set; }
        public string Subspecies { get; set; }
        public string Variety { get; set; }
        public string Subvariety { get; set; }
        public string Form { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
