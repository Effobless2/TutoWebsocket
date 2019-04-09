using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutoWebsocket.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            Console.WriteLine("client connecté et sera identifié par ce token : " + this.Context.ConnectionId);
            await this.Clients.AllExcept(this.Context.ConnectionId).SendAsync("NewConnectedUser", this.Context.ConnectionId);
            await this.Clients.Caller.SendAsync("Connected", this.Context.ConnectionId);
            
        }

        public async Task MessageReceived(string newMessage)
        {
            await this.Clients.All.SendAsync("NewMessage", this.Context.ConnectionId, newMessage);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            await this.Clients.AllExcept(Context.ConnectionId).SendAsync("UserGone", this.Context.ConnectionId);
        }
    }
}
