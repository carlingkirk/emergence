using System;

namespace Emergence.Data.Shared.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public PhotoType Type { get; set; }
        public string Filename { get; set; }
        public string UserId { get; set; }
        public string ContentType { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public DateTime? DateTaken { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Location Location { get; set; }
    }

    public enum PhotoType
    {
        Activity,
        Specimen,
        InventoryItem,
        Origin,
        PlantInfo,
        User
    }
}
