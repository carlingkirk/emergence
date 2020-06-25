using System;
using System.Collections.Generic;
using System.Text;

namespace Emergence.Data.External.iNaturalist
{
    public class Taxon
    {
        public bool is_active { get; set; }
        public string ancestry { get; set; }
        public string min_species_ancestry { get; set; }
        public bool endemic { get; set; }
        public int iconic_taxon_id { get; set; }
        public int min_species_taxon_id { get; set; }
        public bool threatened { get; set; }
        public int rank_level { get; set; }
        public bool introduced { get; set; }
        public bool native { get; set; }
        public int parent_id { get; set; }
        public string name { get; set; }
        public string rank { get; set; }
        public bool extinct { get; set; }
        public int id { get; set; }
        public IEnumerable<int> ancestor_ids { get; set; }
        public int taxon_schemes_count { get; set; }
        public string wikipedia_url { get; set; }
        public object current_synonymous_taxon_ids { get; set; }
        public DateTime created_at { get; set; }
        public int taxon_changes_count { get; set; }
        public object complete_species_count { get; set; }
        public int universal_search_rank { get; set; }
        public int observations_count { get; set; }
        public Flag_Counts flag_counts { get; set; }
        public int? atlas_id { get; set; }
        public Default_Photo default_photo { get; set; }
        public string iconic_taxon_name { get; set; }
        public string preferred_common_name { get; set; }
        public string complete_rank { get; set; }
        public Conservation_Status conservation_status { get; set; }
        public IEnumerable<Ancestor> ancestors { get; set; }
    }
}
