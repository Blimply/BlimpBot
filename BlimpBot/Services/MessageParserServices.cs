﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using BlimpBot.Enums;
using BlimpBot.Interfaces;

namespace BlimpBot.Services
{
    public class MessageParserServices : IMessageParser
    {
        private readonly IWeatherServices _weatherServices;
        private readonly IExchangeRateServices _exchangeRateServices;

        public MessageParserServices(IWeatherServices weatherServices, IExchangeRateServices exchangeRateServices)
        {
            _weatherServices = weatherServices;
            _exchangeRateServices = exchangeRateServices;
        }
        private MessageType GetMessageType(string message)
        {
            if (message.StartsWith("/")) return MessageType.Command;
            return MessageType.Message;
        }

        public string GetResponse(string message)
        {
            var isBlimpSpecific = false;
            if (message.Contains("@"))
            {
                if (!message.ToLower().Contains("blimbpot"))
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
                    response = GetMessageResponse(message);
                    break;
            }

            //url encoding is done by query helper
            return response;
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
                    return _weatherServices.GetWeatherString(arguments);
                case "exchangerates":
                case "exrates":
                case "rates":
                    return _exchangeRateServices.GetExchangeRateString(arguments);
                default:
                    return isBlimpSpecific ? "Command unknown" : string.Empty;
            }
        }

        private string GetMessageResponse(string message)
        {
            var msg = message.ToLower();

            if (!msg.Contains("blimpbot")) return string.Empty;

            if (msg.Contains("hi") || msg.Contains("hello"))
                return "Hello!";

            if (msg.Contains("how are you") || msg.Contains("how're you"))
                return "Feelin' fine";

            return string.Empty;
        }

    }
}
