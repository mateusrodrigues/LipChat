using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace LipChat.Server.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            Clients.Others.receive(message);
        }
    }
}