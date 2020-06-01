using System;
using System.Text.Json.Serialization;

namespace BlimpBot.Models.TelegramResponseModels
{
    public class TelegramUpdate
    {
        public int UpdateId { get; set; }
        [JsonPropertyName("Message")]
        public TelegramMesssage TelegramMesssage { get; set; }
        public DateTime Date { get; set; }


    }
}
