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

        private string GetCommandResponse(string command)
        {
            if (command.Contains("@") && !command.Contains("BlimpBot")) return string.Empty;

            if (command.StartsWith("/weather"))
                return _weatherServices.GetWeatherString();
            if (command.StartsWith("/exchangerates"))
                return _exchangeRateServices.GetExchangeRateString();
            if(command.Contains("@"))
                return "Command Unknown"; //if the command is directed @BlimpBot
            return string.Empty; //maybe they're using some other bot - should consider private messages...
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

        public string GetResponse(string message)
        {
            var response = string.Empty;
            switch (GetMessageType(message))
            {
                case MessageType.Command:
                    response = GetCommandResponse(message);
                    break;
                case MessageType.Message:
                    response = GetMessageResponse(message);
                    break;
            }

            //url encoder is done by query helper
            return response;
        }

        //Fixme - slow?
        //private string EscapeMarkup(string message)
        //{
        //    var escapeChars = new List<string>{"_", "*", "[", "]", "(", ")", "~", "`", ">", "#", "+", "-", "=", "|", "{", "}", ".", "!"};
        //    escapeChars.ToList().ForEach(i=> message = message.Replace(i,@"\"+i));
        //    return message;
        //}
    }
}
