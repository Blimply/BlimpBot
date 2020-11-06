using BlimpBot.Database.Models;
using BlimpBot.Models.TelegramResponseModels;

namespace BlimpBot.Interfaces
{
    public interface IChatBotRepository : IDatabaseRepository
    {
        Chat GetChatByDatabaseId(int id);
        Chat GetChatByTelegramChatId(int telegramChatId);
        bool AddChat(Chat chat);
        bool DeleteChat(Chat chat);
        bool DeleteChat(int id);
        bool CheckIfChatExistsByTelegramChatId(int telegramChatId);
        bool CheckIfChatExists(TelegramChat chat);
        bool CheckIfChatExists(Chat chat);
    }
}
