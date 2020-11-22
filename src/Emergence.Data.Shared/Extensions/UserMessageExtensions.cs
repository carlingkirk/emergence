using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class UserMessageExtensions
    {
        public static Models.UserMessage AsModel(this UserMessage source) => new Models.UserMessage
        {
            Id = source.Id,
            User = source.User?.AsSummaryModel(),
            Sender = source.Sender?.AsSummaryModel(),
            DateSent = source.DateSent,
            Subject = source.Subject,
            MessageBody = source.MessageBody
        };

        public static UserMessage AsStore(this Models.UserMessage source) => new UserMessage
        {
            Id = source.Id,
            UserId = source.User.Id,
            SenderId = source.Sender.Id,
            DateSent = source.DateSent,
            Subject = source.Subject,
            MessageBody = source.MessageBody
        };
    }
}
