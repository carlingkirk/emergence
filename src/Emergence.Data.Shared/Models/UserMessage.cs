using System;

namespace Emergence.Data.Shared.Models
{
    public class UserMessage
    {
        public int Id { get; set; }
        public UserSummary User { get; set; }
        public UserSummary Sender { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public DateTime DateSent { get; set; }

        public string ShortMessage => MessageBody != null ? MessageBody.ToString().Length > 160 ? MessageBody.ToString().Substring(0, 160) + "..." : MessageBody.ToString() : null;
    }
}
