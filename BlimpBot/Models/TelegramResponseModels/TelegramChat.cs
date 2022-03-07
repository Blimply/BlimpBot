namespace BlimpBot.Models.TelegramResponseModels
{
    public class TelegramChat
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

//public class Chat
//{
//    public long id { get; set; }
//    public string title { get; set; }
//    public string type { get; set; }
//}