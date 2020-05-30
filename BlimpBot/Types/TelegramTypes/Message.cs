using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlimpBot.Types
{
    public class Message
    {
        public int message_id { get; set; }
        public Object from { get; set; }
        public Chat chat { get; set; }
        public string text { get; set; }

    }
}
