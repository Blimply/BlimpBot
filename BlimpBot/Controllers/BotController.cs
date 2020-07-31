using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BlimpBot.Constants;
using BlimpBot.Interfaces;
using BlimpBot.Models.TelegramResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BlimpBot.Controllers
{
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly string _token;

        private readonly IMessageParser _messageParser;
        private readonly ITelegramRepository _telegramRepository;

        public BotController(IMessageParser messageParser, IConfiguration configuration, ITelegramRepository telegramRepository)
        {
            //Injected dependencies
            _messageParser = messageParser;
            _telegramRepository = telegramRepository;
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
            return await _telegramRepository.GetBasicInfo();
        }


        [HttpPost("telegram/BlimpBot/{token}")]
        public OkResult HandleWebhook(string token,[FromBody]TelegramUpdate telegramUpdate)
        {
            try
            {
                if (token != _token) return Ok();
                if (telegramUpdate?.TelegramMesssage?.TelegramChat == null) return Ok();

                var chat = telegramUpdate.TelegramMesssage.TelegramChat;
                
                if (telegramUpdate.TelegramMesssage.TelegramChat.Type != ChatTypes.Private)
                    _messageParser.AddUpdateChatListing(chat);

                var response = _messageParser.GetResponse(telegramUpdate.TelegramMesssage.Text);

                if (string.IsNullOrWhiteSpace(response)) return Ok();
                _telegramRepository.SendMessage(response, chat.Id);
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Ok(); //stop telegram spamming our webhook if we fail
            }

            return Ok();

        }

    }
}
