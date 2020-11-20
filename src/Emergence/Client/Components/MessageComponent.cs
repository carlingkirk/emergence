using System;
using System.Threading.Tasks;
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
    }
}
