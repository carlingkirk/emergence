using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListMessagesComponent : ListComponent<UserMessage>
    {
        [Parameter]
        public bool Sent { get; set; }
        protected Dictionary<string, string> GetHeaders() => Sent ?
            new Dictionary<string, string>
            {
                { "DisplayName", "Display Name" },
                { "DateSent", "Date Sent" },
                { "MessageBody", "Message" }
            } :
            new Dictionary<string, string>
            {
                { "SenderName", "Display Name" },
                { "DateSent", "Date Sent" },
                { "MessageBody", "Message" }
            };

        public override async Task<FindResult<UserMessage>> GetListAsync(FindParams findParams)
        {
            FindResult<UserMessage> result;
            if (Sent)
            {
                result = await ApiClient.FindSentMessagesAsync(findParams);
            }
            else
            {
                result = await ApiClient.FindMessagesAsync(findParams);
            }

            return new FindResult<UserMessage>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
