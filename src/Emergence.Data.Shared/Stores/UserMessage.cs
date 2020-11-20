using System;

namespace Emergence.Data.Shared.Stores
{
    public class UserMessage : IIncludable<UserMessage>, IIncludable<UserMessage, User>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SenderId { get; set; }
        public string MessageBody { get; set; }
        public DateTime DateSent { get; set; }
        public User User { get; set; }
        public User Sender { get; set; }
    }
}
