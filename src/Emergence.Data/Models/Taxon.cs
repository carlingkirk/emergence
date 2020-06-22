using Emergence.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emergence.Data.Models
{
    public class Taxon : IClassifiable
    {
        public int TaxonId { get; set; }
        public string Kingdom { get; set; }
        public string Phylum { get; set; }
        public string Subphylum { get; set; }
        public string Class { get; set; }
        public string Subclass { get; set; }
        public string Order { get; set; }
        public string Superfamily { get; set; }
        public string Family { get; set; }
        public string Subfamily { get; set; }
        public string Tribe { get; set; }
        public string Subtribe { get; set; }
        public string Genus { get; set; }
        public string Subgenus { get; set; }
        public string Species { get; set; }
        public string Subspecies { get; set; }
        public string Variety { get; set; }
        public string Form { get; set; }
    }
}
