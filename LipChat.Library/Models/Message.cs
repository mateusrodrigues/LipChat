using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LipChat.Library.Models
{
    public class Message
    {
        public Guid MessageID { get; set; }
        public DateTime DateTime { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
    }
}
