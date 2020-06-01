using System;
using BlimpBot.Models.TelegramResponseModels;

namespace BlimpBot.Interfaces
{
    public interface IMessageParser
    {
        string GetResponse(string message);
        void AddChatListing(TelegramChat telegramChat);
    }
}
