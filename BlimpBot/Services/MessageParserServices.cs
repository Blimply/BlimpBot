using System;
using System.Collections.Generic;
using System.Linq;
using BlimpBot.Constants;
using BlimpBot.Data;
using BlimpBot.Data.Models;
using BlimpBot.Interfaces;
using BlimpBot.Models.TelegramResponseModels;
using Org.BouncyCastle.Utilities.IO;

namespace BlimpBot.Services
{
    public class MessageParserServices : IMessageParser
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly ITelegramRepository _telegramRepository;
        private readonly IChatRepository _chatRepository;

        public MessageParserServices(IWeatherRepository weatherRepository,
                                     IExchangeRateRepository exchangeRateRepository,
                                     ITelegramRepository telegramRepository,
                                     IChatRepository chatRepository)
        {
            _weatherRepository = weatherRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _telegramRepository = telegramRepository;
            _chatRepository = chatRepository;

        }
        public string GetResponse(string message)
        {
            var isBlimpSpecific = false;
            if (message.Contains("@"))
            {
                if (!message.ToLower().Contains("blimpbot"))
                    return string.Empty; //message is for another bot
                isBlimpSpecific = true;
            }

            var response = string.Empty;
            switch (GetMessageType(message))
            {
                case MessageType.Command:
                    var parsedCommand = ParseCommand(message);
                    response = GetCommandResponse(parsedCommand.commandName,parsedCommand.arguments, isBlimpSpecific);
                    break;
                case MessageType.Message:
                    response = GetMessageResponse(message, isBlimpSpecific);
                    break;
            }

            //url encoding is done by query helper
            return response;
        }

        private MessageType GetMessageType(string message)
        {
            if (message.StartsWith("/")) return MessageType.Command;
            return MessageType.Message;
        }

        //Takes format e.g. /commandname arg1 arg2 arg3
        private (string commandName, List<string> arguments) ParseCommand(string message)
        {
            var splitMessage = message.Split(' ');

            //Take everything before the @ and strip out the /
            var command = splitMessage.First()
                                      .Split('@')
                                      .First()
                                      .Substring(1);
            
            return (command,splitMessage.Skip(1).ToList());
            
        }

        private string GetCommandResponse(string commandName, List<string> arguments, bool isBlimpSpecific)
        {
            switch (commandName)
            {
                case "weather":
                    return _weatherRepository.GetWeatherString(arguments);
                case "exchangerates":
                case "exrates":
                case "rates":
                    return _exchangeRateRepository.GetExchangeRateString(arguments);
                default:
                    return isBlimpSpecific ? "Command unknown" : string.Empty;
            }
        }

        private string GetMessageResponse(string message, bool isBlimpSpecific)
        {
            var msg = message.ToLower();

            if (!msg.Contains("blimpbot")) return string.Empty;

            if (msg.Contains("hi ") || msg.Contains("hello "))
                return "Hello!";

            if (msg.Contains("how are you") || msg.Contains("how're you"))
                return "Feelin' fine, thanks fam";

            return isBlimpSpecific ? "I don't understand sorry." : string.Empty;
        }

        public void AddUpdateChatListing(TelegramChat telegramChat)
        {
            if(_chatRepository.CheckIfChatExistsByTelegramChatId(telegramChat.Id))
                UpdateChatListing(telegramChat);
            else
                AddChatListing(telegramChat);
        }

        private void AddChatListing(TelegramChat telegramChat)
        {
            DateTime now = DateTime.Now;

            int memberCount = _telegramRepository.GetChatMemberCount(telegramChat.Id).Value;

            var chatToAdd = new Chat
            {
                ChatId = telegramChat.Id,
                Name = telegramChat.Title,
                MembersCount = memberCount,
                LastMessageReceived = now,
            };

            _chatRepository.AddChat(chatToAdd);
            _chatRepository.SaveChanges();
        }

        public void UpdateChatListing(TelegramChat telegramChat)
        {
            Chat dbChat = _chatRepository.GetChatByTelegramChatId(telegramChat.Id);
            dbChat.MembersCount = _telegramRepository.GetChatMemberCount(telegramChat.Id)
                                                     .Value;

            dbChat.LastMessageReceived = DateTime.Now;
            _chatRepository.SaveChanges();

        }
    }
}
