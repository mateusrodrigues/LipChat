using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace LipChat.Server.Hubs
{
    public class ChatHub : Hub
    {
        public void Hello(string name, string message)
        {
            Clients.All.syncMessage(name, message);
        }
    }
}