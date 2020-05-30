using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlimpBot.Types
{
    public class Chat
    {
        public int id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string username { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}
