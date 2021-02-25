using System;
using System.Collections.Generic;

namespace Emergence.Data.External.NatureServe
{
    public class Plant
    {
        public string ElementGlobalId { get; set; }
        public string UniqueId { get; set; }
        public string NsxUrl { get; set; }
        public string Elcode { get; set; }
        public string ScientificName { get; set; }
        public string FormattedScientificName { get; set; }
        public string PrimaryCommonName { get; set; }
        public string PrimaryCommonNameLanguage { get; set; }
        public string RoundedGRank { get; set; }
        public IEnumerable<Nation> Nations { get; set; }
        public DateTime LastModified { get; set; }
        public string ClassificationStatus { get; set; }
        public SpeciesGlobal SpeciesGlobal { get; set; }
        public string GRank { get; set; }
    }

    public class SpeciesGlobal
    {
        public object UsesaCode { get; set; }
        public object CosewicCode { get; set; }
        public object SaraCode { get; set; }
        public IEnumerable<string> Synonyms { get; set; }
        public IEnumerable<string> OtherCommonNames { get; set; }
        public string Kingdom { get; set; }
        public string Phylum { get; set; }
        public string Taxclass { get; set; }
        public string Taxorder { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public object TaxonomicComments { get; set; }
        public string InformalTaxonomy { get; set; }
        public bool Infraspecies { get; set; }
        public bool CompleteDistribution { get; set; }
    }

    public class Nation
    {
        public string NationCode { get; set; }
        public string RoundedNRank { get; set; }
        public IEnumerable<Subnation> Subnations { get; set; }
        public bool Exotic { get; set; }
        public bool Native { get; set; }
    }

    public class Subnation
    {
        public string SubnationCode { get; set; }
        public string RoundedSRank { get; set; }
        public bool Exotic { get; set; }
        public bool Native { get; set; }
    }
}
