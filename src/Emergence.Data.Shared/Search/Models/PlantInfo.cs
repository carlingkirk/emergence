namespace Emergence.Data.Shared.Search.Models
{
    public class PlantInfo
    {
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public bool? Preferred { get; set; }
        public BloomTime BloomTime { get; set; }
        public Height Height { get; set; }
        public Spread Spread { get; set; }
        public Requirements Requirements { get; set; }
        public Visibility Visibility { get; set; }
    }
}
