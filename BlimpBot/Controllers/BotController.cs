using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlimpBot.Constants;
using BlimpBot.Interfaces;
using BlimpBot.Models.TelegramResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace BlimpBot.Controllers
{
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly string _token;

        private readonly IMessageParser _messageParser;
        private readonly ITelegramServices _telegramServices;

        public BotController(IMessageParser messageParser, IConfiguration configuration, ITelegramServices telegramServices)
        {
            //Injected dependencies
            _messageParser = messageParser;
            _telegramServices = telegramServices;
            _token = configuration["BlimpBotTelegramToken"];
        }


        [HttpGet("welcome")]
        public ActionResult<string> GetWelcomeMessage()
        {
            return "Welcome to the api!";
        }

        [HttpGet("BasicInfo")]
        public async Task<ActionResult<string>> GetBasicInfoAsync()
        {
            return await _telegramServices.GetBasicInfo();
        }


        [HttpPost("telegram/BlimpBot/{token}")]
        public void HandleWebhook(string token,[FromBody]TelegramUpdate telegramUpdate)
        {
            if (token != _token) return;
            

            var chat = telegramUpdate.TelegramMesssage.TelegramChat;
            if (telegramUpdate.TelegramMesssage.TelegramChat.Type != ChatTypes.Private)
                _messageParser.AddChatListing(chat);

            var response = _messageParser.GetResponse(telegramUpdate.TelegramMesssage.Text);

            if (string.IsNullOrWhiteSpace(response)) return;
            _telegramServices.SendMessage(response, chat.Id);

        }

    }
}
