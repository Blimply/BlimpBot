using System;

namespace BlimpBot.Models.TelegramResponseModels
{
    public class Message
    {
        public int message_id { get; set; }
        public Object from { get; set; }
        public Chat chat { get; set; }
        public string text { get; set; }

    }
}
