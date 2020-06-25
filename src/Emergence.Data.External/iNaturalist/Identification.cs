using System;
using System.Collections.Generic;
using System.Text;

namespace Emergence.Data.External.iNaturalist
{
    public class Identification
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public User user { get; set; }
        public DateTime created_at { get; set; }
        public Created_At_Details created_at_details { get; set; }
        public string body { get; set; }
        public string category { get; set; }
        public bool current { get; set; }
        public IEnumerable<object> flags { get; set; }
        public bool own_observation { get; set; }
        public object taxon_change { get; set; }
        public bool vision { get; set; }
        public bool? disagreement { get; set; }
        public int? previous_observation_taxon_id { get; set; }
        public bool spam { get; set; }
        public int taxon_id { get; set; }
        public bool hidden { get; set; }
        public IEnumerable<object> moderator_actions { get; set; }
        public Taxon taxon { get; set; }
        public Previous_Observation_Taxon previous_observation_taxon { get; set; }
    }
}
