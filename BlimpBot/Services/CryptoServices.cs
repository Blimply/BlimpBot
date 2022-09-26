using System;
using System.Collections.Generic;
using System.Net.Http;
using BlimpBot.Interfaces;
using BlimpBot.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace BlimpBot.Services
{
    public class CryptoServices : ICryptoServices
    {
        private readonly HttpClient _client;
        public CryptoServices(HttpClient client)
        {
            _client = client;
        }

        public OurChatResponse GetChatResponse(List<string> argumentsList)
        {
            if (argumentsList.Count == 0)
                argumentsList = new List<string>{"bitcoin", "dogecoin", "banano", "fantom"};
            
            var response = GetCryptoRate(string.Join(",", argumentsList));
            var timestamp = DateTime.UtcNow.AddHours(8);

            var outString = $"Current value of cryptos in AUD as of {timestamp:dddd, dd MMMM yyyy HH:mm:ss} (AWST)\n";
            foreach ((string key, Dictionary<string, float> valueDict) in response)
            {
                outString += $"{key}: ${valueDict["aud"]}\n";
            }

            return new OurChatResponse{ Text = outString };
        }

        private Dictionary<string, Dictionary<string, float>> GetCryptoRate(string cryptoNames)
        {
            var coinGeckoUrl = "https://api.coingecko.com/api/v3/simple/price";
            //?ids=banano,bitcoin&vs_currencies=aud
            var query = new Dictionary<string, string>
            {
                ["ids"] = cryptoNames,
                ["vs_currencies"] = "aud",
            };
            var request = QueryHelpers.AddQueryString(coinGeckoUrl, query);
            var cryptoResponse = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, float>>>(_client.GetStringAsync(request).Result);

            return cryptoResponse;

        }
    }
}
