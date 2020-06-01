using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
        private readonly HttpClient _client;
        private readonly string _token;
        private readonly string _telegramBaseUri = "https://api.telegram.org/bot";

        private readonly IMessageParser _messageParser;
        private readonly IConfiguration _configuration;

        public BotController(HttpClient httpClient, IMessageParser messageParser, IConfiguration configuration)
        {
            //Injected dependencies
            _messageParser = messageParser;
            _client = httpClient;
            _configuration = configuration;
            
            _token = _configuration["BlimpBotTelegramToken"];
        }


        [HttpGet("welcome")]
        public ActionResult<string> GetWelcomeMessage()
        {
            return "Welcome to the api!";
        }

        [HttpGet("BasicInfo")]
        public async Task<ActionResult<string>> GetBasicInfoAsync()
        {
            var responseString = await _client.GetStringAsync(_telegramBaseUri + _token+"/getMe");
            return responseString;
        }


        [HttpPost("telegram/BlimpBot/{token}")]
        public async void HandleWebhook(string token,[FromBody]Update update)
        {
            if (token != _token) return;
            var response = _messageParser.GetResponse(update.message.text);
            if (string.IsNullOrWhiteSpace(response)) return;
            SendMessage(response, update.message.chat.id);
        }

        private async void SendMessage(string message, int chatId)
        {
            var query = new Dictionary<string, string>
            {
                ["chat_id"] = chatId.ToString(),
                ["text"] = message,
                ["parse_mode"] = "HTML",
            };
            var request = QueryHelpers.AddQueryString($"{_telegramBaseUri}{_token}/sendMessage", query);
            //Console.WriteLine(request);
            var response = await _client.GetAsync(request);
        }
    }
}
