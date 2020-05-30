using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlimpBot.Types
{
    public class Update
    {
        public int update_id { get; set; }
        public Message message { get; set; }
        public DateTime date { get; set; }


    }
}
