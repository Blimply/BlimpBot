using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlimpBot.Models.DatabaseModels
{
    public class Chat
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int MembersCount { get; set; }
        public DateTime? LastMessageReceived { get; set; }
    }
}
