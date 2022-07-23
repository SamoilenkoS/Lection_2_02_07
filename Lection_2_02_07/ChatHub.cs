using Lection_2_Core;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_02_07
{
    public class ChatHub : Hub<IClientHub>, IServerHub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Others.ReceiveMessage(Context.ConnectionId, " has been connected!");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.ReceiveMessage(Context.ConnectionId, " has been disconnected!");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.Others.ReceiveMessage(user, message);
        }
    }
}
