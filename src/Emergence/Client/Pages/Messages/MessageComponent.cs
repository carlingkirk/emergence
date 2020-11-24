using System;
using System.Threading.Tasks;
using Blazored.Modal;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class MessageComponent : ViewerComponent<UserMessage>
    {
        [Parameter]
        public UserMessage Message { get; set; }
        [Parameter]
        public Func<Task> Cancel { get; set; }
        [Parameter]
        public UserSummary Recipient { get; set; }
        [Parameter]
        public string Subject { get; set; }
        [Parameter]
        public bool IsSent { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        public UserSummary SelectedUser { get; set; }
        public bool IsReplying { get; set; }
        public UserMessage ReplyMessage { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Message == null)
            {
                IsEditing = true;

                if (Recipient != null)
                {
                    SelectedUser = Recipient;
                }

                if (Message == null)
                {
                    Message = new UserMessage
                    {
                        User = SelectedUser,
                        Subject = Subject
                    };
                }
            }
            else
            {
                if (Message.Id == 0)
                {
                    IsEditing = true;
                }

                SelectedUser = Message.User;
            }
        }

        protected void Reply()
        {
            var quotedBody = "> " + Message.MessageBody.Replace(Environment.NewLine, Environment.NewLine + "> ");
            ReplyMessage = new UserMessage
            {
                User = IsSent ? Message.User : Message.Sender,
                Subject = "Re: " + Message.Subject,
                MessageBody = $"{Environment.NewLine}{Environment.NewLine}> On {Message.DateSent.ToShortDateString()} at {Message.DateSent.ToShortTimeString()}, " +
                $"{Message.Sender.DisplayName} wrote {Environment.NewLine}{quotedBody}"
            };

            IsReplying = true;
        }
    }
}
