using Microsoft.AspNetCore.SignalR;

namespace Emergence.Models
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection) => connection.User?.Identity?.Name;
    }
}
