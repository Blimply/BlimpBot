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


//public class Message
//{
//    public int message_id { get; set; }
//    public From from { get; set; }
//    public Chat chat { get; set; }
//    public int date { get; set; }
//    public Forward_From forward_from { get; set; }
//    public int forward_date { get; set; }
//    public Video video { get; set; }
//    public string caption { get; set; }
//    public Caption_Entities[] caption_entities { get; set; }
//}

//public class From
//{
//    public int id { get; set; }
//    public bool is_bot { get; set; }
//    public string first_name { get; set; }
//    public string last_name { get; set; }
//    public string username { get; set; }
//}



//public class Forward_From
//{
//    public int id { get; set; }
//    public bool is_bot { get; set; }
//    public string first_name { get; set; }
//    public string username { get; set; }
//}

//public class Video
//{
//    public int duration { get; set; }
//    public int width { get; set; }
//    public int height { get; set; }
//    public string file_name { get; set; }
//    public string mime_type { get; set; }
//    public Thumb thumb { get; set; }
//    public string file_id { get; set; }
//    public string file_unique_id { get; set; }
//    public int file_size { get; set; }
//}

//public class Thumb
//{
//    public string file_id { get; set; }
//    public string file_unique_id { get; set; }
//    public int file_size { get; set; }
//    public int width { get; set; }
//    public int height { get; set; }
//}

//public class Caption_Entities
//{
//    public int offset { get; set; }
//    public int length { get; set; }
//    public string type { get; set; }
//    public string url { get; set; }
//}
