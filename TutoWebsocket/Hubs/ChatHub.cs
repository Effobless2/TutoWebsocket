using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutoWebsocket.Hubs
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// Appelé quand qqn se connecte au hub
        /// </summary>
        /// <returns></returns>        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            //this.Context.ConnectionId est le jeton, l'identifiant de la connexion et cera toujours utilisé pour identifié le client qui vient d'appeler la méthode
            //Il sera stocké ensuite dans la variable Clients.All pour ensuite pouvoir être utilisé.
            Console.WriteLine("client connecté et sera identifié par ce token : " + this.Context.ConnectionId); 
            //SendAsync("Msg",params) envoie au(x) client(s) concerné(s) la requête "Msg" suivi de params. C'est tout x)
            await this.Clients.AllExcept(this.Context.ConnectionId).SendAsync("NewConnectedUser", this.Context.ConnectionId);
            await this.Clients.Caller.SendAsync("Connected", this.Context.ConnectionId);
            
        }

        /// <summary>
        /// Appelé quand qqn envoie un message
        /// </summary>
        /// <param name="newMessage"></param>
        /// <returns></returns>
        public async Task MessageReceived(string newMessage)
        {
            //et oui on peut mettre plusieurs paramètres. Il faut juste que le premier soit un string et il permettra d'identifier le type de message envoyé par le serveur.
            await this.Clients.All.SendAsync("NewMessage", this.Context.ConnectionId, newMessage);
        }

        /// <summary>
        /// Appelé quand qqn se déco
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            await this.Clients.AllExcept(Context.ConnectionId).SendAsync("UserGone", this.Context.ConnectionId);
        }
    }
}
