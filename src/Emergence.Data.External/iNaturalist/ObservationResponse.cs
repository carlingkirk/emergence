using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emergence.Data.External.iNaturalist
{
    public class ObservationResponse
    {
        public int total_results { get; set; }
        public int page { get; set; }
        public int per_page { get; set; }
        [JsonProperty("results")]
        public IEnumerable<Observation> Observations { get; set; }
    }
}
