using System;
using System.Text.Json.Serialization;

namespace BlimpBot.Models.TelegramResponseModels
{
    public class TelegramMesssage
    {
        public int MessageId { get; set; }
        public Object From { get; set; }
        [JsonPropertyName("Chat")]
        public TelegramChat TelegramChat { get; set; }
        public string Text { get; set; }

    }
}
