using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using BlimpBot.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlimpBot.Services
{
    public class TelegramRepository : ITelegramRepository
    {
        private readonly string _token;
        private readonly HttpClient _client;
        private readonly ILogger<TelegramRepository> _logger;
        private readonly string _telegramBaseUri = "https://api.telegram.org/bot";

        public TelegramRepository(HttpClient client, IConfiguration configuration, ILogger<TelegramRepository> logger)
        {
            _client = client;
            _logger = logger;
            _token = configuration["BlimpBotTelegramToken"];
        }
        public async Task<ActionResult<string>> GetBasicInfo()
        {
            var responseString = await _client.GetStringAsync(_telegramBaseUri + _token + "/getMe");
            _logger.LogWarning($"Got basic info!");
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
            _logger.LogWarning($"Request url {request}");
            return await _client.GetStringAsync(request);
        }

        public ActionResult<int> GetChatMemberCount(int chatId)
        {
            var query = new Dictionary<string, string>
            {
                ["chat_id"] = chatId.ToString(),
            };
            var request = QueryHelpers.AddQueryString($"{_telegramBaseUri}{_token}/getChatMembersCount", query);
            var response = _client.GetStringAsync(request).Result;
            var memberCount = JsonSerializer.Deserialize<ChatMemberCountResponse>(response);

            return memberCount.Result;
        }

        //TODO: new class
        private class ChatMemberCountResponse
        {
            public bool Ok { get; set; }
            public int Result { get; set; }
        }
    }
}
