using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LipChat.Server.Models
{
    public class Message
    {
        public Guid MessageID { get; set; }
        public string Content { get; set; }
    }
}