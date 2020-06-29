using System;
using System.Collections.Generic;

namespace Emergence.Data.External.iNaturalist
{
    public class User
    {
        public DateTime created_at { get; set; }
        public int id { get; set; }
        public string login { get; set; }
        public bool spam { get; set; }
        public bool suspended { get; set; }
        public string login_autocomplete { get; set; }
        public string login_exact { get; set; }
        public string name { get; set; }
        public string name_autocomplete { get; set; }
        public object orcid { get; set; }
        public string icon { get; set; }
        public int observations_count { get; set; }
        public int identifications_count { get; set; }
        public int journal_posts_count { get; set; }
        public int activity_count { get; set; }
        public int universal_search_rank { get; set; }
        public IEnumerable<object> roles { get; set; }
        public object site_id { get; set; }
        public string icon_url { get; set; }
        public Preferences preferences { get; set; }
    }
}
