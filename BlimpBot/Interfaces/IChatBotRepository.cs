using BlimpBot.Database.Models;
using BlimpBot.Models.TelegramResponseModels;

namespace BlimpBot.Interfaces
{
    public interface IChatBotRepository : IDatabaseRepository
    {
        Chat GetChatByDatabaseId(int id);
        Chat GetChatByTelegramChatId(string telegramChatId);
        bool AddChat(Chat chat);
        bool DeleteChat(Chat chat);
        bool DeleteChat(int id);
        bool CheckIfChatExistsByTelegramChatId(string telegramChatId);
        bool CheckIfChatExists(TelegramChat chat);
        bool CheckIfChatExists(Chat chat);
    }
}
