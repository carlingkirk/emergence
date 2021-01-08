using System;

namespace Emergence.Data.Shared.Search.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? TypeId { get; set; }
        public string Filename { get; set; }
        public string BlobPath { get; set; }
        public string ContentType { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DateTime? DateTaken { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Location Location { get; set; }
    }
}
