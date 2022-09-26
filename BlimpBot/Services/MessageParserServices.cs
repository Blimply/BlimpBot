using System;
using System.Collections.Generic;
using System.Linq;
using BlimpBot.Constants;
using BlimpBot.Database.Models;
using BlimpBot.Interfaces;
using BlimpBot.Models;
using BlimpBot.Models.TelegramResponseModels;

namespace BlimpBot.Services
{
    public class MessageParserServices : IMessageParser
    {
        private readonly IWeatherServices _weatherServices;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ITelegramRepository _telegramRepository;
        private readonly IChatBotRepository _chatBotRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ICryptoServices _cryptoServices;
        private readonly IMinorApiServices _minorApiServices;

        public MessageParserServices(IWeatherServices weatherServices,
                                     IExchangeRateService exchangeRateService,
                                     ITelegramRepository telegramRepository,
                                     IChatBotRepository chatBotRepository,
                                     IReviewRepository reviewRepository,
                                     ICryptoServices cryptoServices,
                                     IMinorApiServices minorApiServices)
        {
            _weatherServices = weatherServices;
            _exchangeRateService = exchangeRateService;
            _telegramRepository = telegramRepository;
            _chatBotRepository = chatBotRepository;
            _reviewRepository = reviewRepository;
            _cryptoServices = cryptoServices;
            _minorApiServices = minorApiServices;
        }
        public OurChatResponse GetChatResponse(string message)
        {
            var isBlimpSpecific = false;
            if (message.Contains("@"))
            {
                if (!message.ToLower().Contains("blimpbot"))
                    return new OurChatResponse(); //message is for another bot
                isBlimpSpecific = true;
            }

            OurChatResponse response = new OurChatResponse();
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

        private OurChatResponse GetCommandResponse(string commandName, List<string> arguments, bool isBlimpSpecific)
        {
            IChatCommandRepository repository = null;

            var apiType = MinorApiType.None;
            switch (commandName)
            {
                case "weather":
                    repository = _weatherServices;
                    break;
                case "exchangerates":
                case "exrates":
                case "rates":
                    repository = _exchangeRateService;
                    break;
                case "cryptos":
                case "coins":
                case "crypto":
                    repository = _cryptoServices;
                    break;
                case "review":
                case "reviews":
                    repository = _reviewRepository;
                    break;
                case "woof":
                case "dog":
                    apiType = MinorApiType.Dog;
                    repository = _minorApiServices;
                    break;
                case "duck":
                case "quack":
                    apiType = MinorApiType.Duck;
                    repository = _minorApiServices;
                    break;
                case "cat":
                case "meow":
                    apiType = MinorApiType.Cat;
                    repository = _minorApiServices;
                    break;
                case "coffee":
                    apiType = MinorApiType.Coffee;
                    repository = _minorApiServices;
                    break;
            }

            if (repository != null)
                return repository.GetChatResponse(arguments, apiType);
            return isBlimpSpecific ? new OurChatResponse{Text="Command unknown"} : new OurChatResponse();
        }

        private OurChatResponse GetMessageResponse(string message, bool isBlimpSpecific)
        {
            var msg = message.ToLower();

            if (!msg.Contains("blimpbot")) return new OurChatResponse();

            if (msg.Contains("hi ") || msg.Contains("hello "))
                return new OurChatResponse{Text="Hello!"};

            if (msg.Contains("how are you") || msg.Contains("how're you"))
                return new OurChatResponse{Text="Feelin' fine, thanks fam"};

            return isBlimpSpecific ? new OurChatResponse{Text="I don't understand sorry."} : new OurChatResponse();
        }

        public void AddUpdateChatListing(TelegramChat telegramChat)
        {
            if(_chatBotRepository.CheckIfChatExistsByTelegramChatId(telegramChat.Id.ToString()))
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
                ChatId = telegramChat.Id.ToString(),
                Name = telegramChat.Title,
                MembersCount = memberCount,
                LastMessageReceived = now,
            };

            _chatBotRepository.AddChat(chatToAdd);
            _chatBotRepository.SaveChanges();
        }

        public void UpdateChatListing(TelegramChat telegramChat)
        {
            Chat dbChat = _chatBotRepository.GetChatByTelegramChatId(telegramChat.Id.ToString());
            dbChat.MembersCount = _telegramRepository.GetChatMemberCount(telegramChat.Id)
                                                     .Value;
            dbChat.Name = telegramChat.Title;
            dbChat.LastMessageReceived = DateTime.Now;
            _chatBotRepository.SaveChanges();

        }
    }
}
