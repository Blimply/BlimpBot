namespace BlimpBot.Models.TelegramResponseModels
{
    public class TelegramChat
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
