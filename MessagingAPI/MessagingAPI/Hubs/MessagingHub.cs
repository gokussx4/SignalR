using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingAPI.Hubs
{
    public class MessagingHub : Hub
    {
        private static HashSet<string> connectedUsers = new HashSet<string>();

        public override Task OnConnected()
        {
            connectedUsers.Add(Context.ConnectionId);
            UserCount();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            connectedUsers.Remove(Context.ConnectionId);
            UserCount();
            return base.OnDisconnected(stopCalled);
        }

        public int UserCount()
        {
            Clients.All.sendUserCount(connectedUsers.Count);
            return connectedUsers.Count;
        }

        public void UserIsTyping()
        {
            Clients.All.sendUserTyping(Context.User.Identity.Name);
        }
    }
}
