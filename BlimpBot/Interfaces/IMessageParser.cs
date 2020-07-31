using System;
using BlimpBot.Models.TelegramResponseModels;

namespace BlimpBot.Interfaces
{
    public interface IMessageParser
    {
        string GetResponse(string message);
        void AddUpdateChatListing(TelegramChat telegramChat); //These should be moved into a services somewhere
    }
}
