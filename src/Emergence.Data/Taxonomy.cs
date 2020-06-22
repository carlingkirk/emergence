using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emergence.Data
{
    public class Taxononomy
    {
        public string PreferredCommonName { get; set; }
        public string Name { get; set; }
        public IEnumerable<TaxonClassification> Classifications { get; set; }
        public TaxonClassification Classification => Classifications.FirstOrDefault(c => c.Preferred) ?? Classifications.FirstOrDefault();

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class TaxonClassification
    {
        public TaxonClassification()
        {

        }

        private Dictionary<int, int> _assignment { get; set; }
        public long TaxonId { get; set; }
        public TaxonType TaxonType { get; set; }
        public long SourceId { get; set; }
        public bool Preferred { get; set; }
    }

    public enum TaxonType
    {
        Kingdom,
        Phylum,
        Subphylum,
        Class,
        Subclass,
        Order,
        Superfamily,
        Tribe,
        Family,
        Subfamily,
        Genus,
        Subgenus,
        Species,
        Subspecies,
        Variety,
        Form
    }

    /*
     * 
     * Storage considerations
     * A taxon record can store a little as one classification or the entire classification
     * A plant profile could have multiple classifications - should include a source
     * Key Value pairs?
     * Arrays in JSON?
     * TaxonId 1
     * TaxonClassification - [TaxonId] [0,1]
     * 
     * Taxon
     * TaxonId
     * TaxonTypeId
     * TaxonName
     * 
     * MetaTaxon?
     * Could store the structure without being related to a plant
     * But couldn't we just calculate it with classification data?
     * 
     * TaxonId
     * Id    ParentId  Kingdom  Phylum      Subphylum
     * 1     null      Plants   null        null
     * 2     1         Plants   adjkrfkad   null
     * 3     2         Plants   adjkrfkad   dsfdfg
     */
}
