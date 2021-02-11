using System;

namespace Emergence.Data.Shared.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public PhotoType Type { get; set; }
        public int? TypeId { get; set; }
        public string Filename { get; set; }
        public string BlobPath { get; set; }
        public string ExternalUrl { get; set; }
        public string UserId { get; set; }
        public string ContentType { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DateTime? DateTaken { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Location Location { get; set; }
        public string BlobPathRoot { get; set; }
        public string OriginalUri =>
            !string.IsNullOrEmpty(BlobPathRoot) && !string.IsNullOrEmpty(BlobPath) && !string.IsNullOrEmpty(Filename)
                ? BlobPathRoot + BlobPath + "/" + Filename : null;
        public string LargeUri => !string.IsNullOrEmpty(BlobPathRoot) && !string.IsNullOrEmpty(BlobPath) && !string.IsNullOrEmpty(Filename)
                ? BlobPathRoot + BlobPath + "/" + "large.png" : null;
        public string MediumUri => !string.IsNullOrEmpty(BlobPathRoot) && !string.IsNullOrEmpty(BlobPath) && !string.IsNullOrEmpty(Filename)
                ? BlobPathRoot + BlobPath + "/" + "medium.png" : null;
        public string ThumbnailUri => !string.IsNullOrEmpty(BlobPathRoot) && !string.IsNullOrEmpty(BlobPath) && !string.IsNullOrEmpty(Filename)
                ? BlobPathRoot + BlobPath + "/" + "thumb.png" : null;
    }
}
