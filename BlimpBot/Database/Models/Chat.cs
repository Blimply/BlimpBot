using System;

namespace BlimpBot.Database.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Name { get; set; }
        public int MembersCount { get; set; }
        public DateTime? LastMessageReceived { get; set; }
    }
}
