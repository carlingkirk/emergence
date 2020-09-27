using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Photo : IAuditable
    {
        public int Id { get; set; }
        [StringLength(36)]
        public string Type { get; set; }
        public int? TypeId { get; set; }
        public int? LocationId { get; set; }
        [StringLength(200)]
        public string Filename { get; set; }
        [StringLength(500)]
        public string BlobPath { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DateTime? DateTaken { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Location Location { get; set; }
    }
}
