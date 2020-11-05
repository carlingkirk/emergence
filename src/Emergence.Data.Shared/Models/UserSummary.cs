namespace Emergence.Data.Shared.Models
{
    public class UserSummary
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int? PhotoId { get; set; }
        public string PhotoThumbnailUri { get; set; }
        public Visibility Visibility { get; set; }
    }
}
