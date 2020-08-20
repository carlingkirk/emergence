using Microsoft.AspNetCore.SignalR;

namespace Emergence.Client.Server
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection) => connection.User?.Identity?.Name;
    }
}
