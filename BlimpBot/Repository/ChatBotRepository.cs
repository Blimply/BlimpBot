using System.Linq;
using BlimpBot.Database;
using BlimpBot.Database.Models;
using BlimpBot.Interfaces;
using BlimpBot.Models.TelegramResponseModels;

namespace BlimpBot.Repository
{
    public class ChatBotRepository : BaseRepository, IChatBotRepository
    {
        private readonly BlimpBotContext _context;

        public ChatBotRepository(BlimpBotContext context)
        {
            Context = context;
            _context = context;
        }

        public Chat GetChatByDatabaseId(int id)
        {
            return _context.Chats.Find(id);
        }

        public Chat GetChatByTelegramChatId(int telegramChatId)
        {
            return _context.Chats.FirstOrDefault(i => i.ChatId == telegramChatId);
        }

        public bool AddChat(Chat chat)
        {
            if (CheckIfChatExists(chat)) return false;
            _context.Chats.Add(chat);
            return true;
        }

        public bool DeleteChat(Chat chat)
        {
            if (!CheckIfChatExists(chat)) return false;
            _context.Chats.Remove(chat);
            return true;
        }

        public bool DeleteChat(int id)
        {
            return DeleteChat(GetChatByDatabaseId(id));
        }

        public bool CheckIfChatExistsByTelegramChatId(int telegramChatId)
        {
            return _context.Chats.FirstOrDefault(i=>i.ChatId == telegramChatId) != null;
        }

        public bool CheckIfChatExists(TelegramChat chat)
        {
            return CheckIfChatExistsByTelegramChatId(chat.Id);
        }

        public bool CheckIfChatExists(Chat chat)
        {
            return CheckIfChatExistsByTelegramChatId(chat.ChatId);
        }
    }
}
