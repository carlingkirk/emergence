namespace Emergence.Data.Shared.Models.Places
{
    public class Geometry
    {
        public PlaceLocation Location { get; set; }
        public ViewPort Bounds { get; set; }
        public ViewPort ViewPort { get; set; }
        public GeometryLocationType LocationType { get; set; }
    }

    public class PlaceLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
    }

    public enum GeometryLocationType
    {

        Rooftop = 0,
        RangeInterpolated = 1,
        GeometricCenter = 2,
        Approximate = 3
    }
}
