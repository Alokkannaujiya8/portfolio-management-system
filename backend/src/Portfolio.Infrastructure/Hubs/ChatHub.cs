using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> _connections = new();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, DateTime.Now.ToString("HH:mm"));
        }

        public async Task JoinChat(string userName)
        {
            _connections[Context.ConnectionId] = userName;
            await Clients.All.SendAsync("UserJoined", userName);
            await Clients.All.SendAsync("UpdateUserCount", _connections.Count);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out var userName))
            {
                _connections.Remove(Context.ConnectionId);
                await Clients.All.SendAsync("UserLeft", userName);
                await Clients.All.SendAsync("UpdateUserCount", _connections.Count);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
