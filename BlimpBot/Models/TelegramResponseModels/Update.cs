using System;

namespace BlimpBot.Models.TelegramResponseModels
{
    public class Update
    {
        public int update_id { get; set; }
        public Message message { get; set; }
        public DateTime date { get; set; }


    }
}
