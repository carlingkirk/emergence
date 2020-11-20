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
        protected static Dictionary<string, string> Headers =>
            new Dictionary<string, string>
            {
                { "DisplayName", "Display Name" },
                { "DateSent", "Date Sent" }
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
