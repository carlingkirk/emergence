using System;
using System.Collections.Generic;

namespace Emergence.Data.External.iNaturalist
{
#pragma warning disable IDE1006 // Naming Styles
    public class Observation
    {
        public bool? out_of_range { get; set; }
        public string quality_grade { get; set; }
        public DateTime? time_observed_at { get; set; }
        public string taxon_geoprivacy { get; set; }
        public IEnumerable<object> annotations { get; set; }
        public object context_user_geoprivacy { get; set; }
        public string uuid { get; set; }
        public Observed_On_Details observed_on_details { get; set; }
        public int id { get; set; }
        public int cached_votes_total { get; set; }
        public bool identifications_most_agree { get; set; }
        public Created_At_Details created_at_details { get; set; }
        public string species_guess { get; set; }
        public bool identifications_most_disagree { get; set; }
        public IEnumerable<object> tags { get; set; }
        public int? positional_accuracy { get; set; }
        public int comments_count { get; set; }
        public int site_id { get; set; }
        public string created_time_zone { get; set; }
        public bool id_please { get; set; }
        public object license_code { get; set; }
        public string observed_time_zone { get; set; }
        public IEnumerable<object> quality_metrics { get; set; }
        public int? public_positional_accuracy { get; set; }
        public IEnumerable<int> reviewed_by { get; set; }
        public object context_geoprivacy { get; set; }
        public int? oauth_application_id { get; set; }
        public IEnumerable<object> flags { get; set; }
        public DateTime created_at { get; set; }
        public object description { get; set; }
        public string time_zone_offset { get; set; }
        public IEnumerable<object> project_ids_with_curator_id { get; set; }
        public string observed_on { get; set; }
        public string observed_on_string { get; set; }
        public DateTime updated_at { get; set; }
        public IEnumerable<object> sounds { get; set; }
        public IEnumerable<int> place_ids { get; set; }
        public bool captive { get; set; }
        public Taxon taxon { get; set; }
        public IEnumerable<int> ident_taxon_ids { get; set; }
        public IEnumerable<object> outlinks { get; set; }
        public int faves_count { get; set; }
        public object context_taxon_geoprivacy { get; set; }
        public IEnumerable<object> ofvs { get; set; }
        public int num_identification_agreements { get; set; }
        public Preferences preferences { get; set; }
        public IEnumerable<object> comments { get; set; }
        public object map_scale { get; set; }
        public string uri { get; set; }
        public IEnumerable<object> project_ids { get; set; }
        public int? community_taxon_id { get; set; }
        public Geojson geojson { get; set; }
        public bool? owners_identification_from_vision { get; set; }
        public int identifications_count { get; set; }
        public bool obscured { get; set; }
        public int num_identification_disagreements { get; set; }
        public object geoprivacy { get; set; }
        public string location { get; set; }
        public IEnumerable<object> votes { get; set; }
        public bool spam { get; set; }
        public User user { get; set; }
        public bool mappable { get; set; }
        public bool identifications_some_agree { get; set; }
        public IEnumerable<object> project_ids_without_curator_id { get; set; }
        public string place_guess { get; set; }
        public Identification[] identifications { get; set; }
        public IEnumerable<object> project_observations { get; set; }
        public IEnumerable<Photo> photos { get; set; }
        public IEnumerable<Observation_Photos> observation_photos { get; set; }
        public IEnumerable<object> faves { get; set; }
        public IEnumerable<Non_Owner_Ids> non_owner_ids { get; set; }
    }

    public class Observed_On_Details
    {
        public string date { get; set; }
        public int week { get; set; }
        public int month { get; set; }
        public int hour { get; set; }
        public int year { get; set; }
        public int day { get; set; }
    }

    public class Created_At_Details
    {
        public string date { get; set; }
        public int week { get; set; }
        public int month { get; set; }
        public int hour { get; set; }
        public int year { get; set; }
        public int day { get; set; }
    }

    public class Flag_Counts
    {
        public int unresolved { get; set; }
        public int resolved { get; set; }
    }

    public class Default_Photo
    {
        public string square_url { get; set; }
        public string attribution { get; set; }
        public IEnumerable<object> flags { get; set; }
        public string medium_url { get; set; }
        public int id { get; set; }
        public string license_code { get; set; }
        public Original_Dimensions original_dimensions { get; set; }
        public string url { get; set; }
    }

    public class Original_Dimensions
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Conservation_Status
    {
        public string status_name { get; set; }
        public int iucn { get; set; }
        public string authority { get; set; }
        public object geoprivacy { get; set; }
        public int source_id { get; set; }
        public object place_id { get; set; }
        public string status { get; set; }
    }

    public class Preferences
    {
        public bool auto_obscuration { get; set; }
        public object prefers_community_taxon { get; set; }
    }

    public class Geojson
    {
        public IEnumerable<float> coordinates { get; set; }
        public string type { get; set; }
    }

    public class Photo
    {
        public int id { get; set; }
        public object license_code { get; set; }
        public string url { get; set; }
        public string attribution { get; set; }
        public Original_Dimensions original_dimensions { get; set; }
        public IEnumerable<object> flags { get; set; }
    }

    public class Observation_Photos
    {
        public int id { get; set; }
        public int position { get; set; }
        public string uuid { get; set; }
        public Photo photo { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
