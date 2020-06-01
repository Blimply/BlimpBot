using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlimpBot.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace BlimpBot.Services
{
    public class TelegramServices : ITelegramServices
    {
        private readonly string _token;
        private readonly HttpClient _client;
        private readonly string _telegramBaseUri = "https://api.telegram.org/bot";

        public TelegramServices(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _token = configuration["BlimpBotTelegramToken"];
        }
        public async Task<ActionResult<string>> GetBasicInfo()
        {
            var responseString = await _client.GetStringAsync(_telegramBaseUri + _token + "/getMe");
            return responseString;
        }
        public async Task<ActionResult<string>> SendMessage(string message, int chatId)
        {
            var query = new Dictionary<string, string>
            {
                ["chat_id"] = chatId.ToString(),
                ["text"] = message,
                ["parse_mode"] = "HTML",
            };
            var request = QueryHelpers.AddQueryString($"{_telegramBaseUri}{_token}/sendMessage", query);
            //Console.WriteLine(request);
            return await _client.GetStringAsync(request);
        }

        public Task<ActionResult<int>> GetChatMemberCount(int chatId)
        {
            throw new NotImplementedException();
        }

    }
}
