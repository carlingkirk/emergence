using System;

namespace Emergence.Data.Shared.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public PhotoType Type { get; set; }
        public int? TypeId { get; set; }
        public string Filename { get; set; }
        public string UserId { get; set; }
        public string ContentType { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public DateTime? DateTaken { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Location Location { get; set; }
        public string AbsoluteUri { get; set; }
        public string RelativeUri => Type.ToString().ToLower() + "/" + Filename;
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
