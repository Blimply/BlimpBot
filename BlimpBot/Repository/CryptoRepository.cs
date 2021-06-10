﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using BlimpBot.Interfaces;
using BlimpBot.Models.ExchangeRateModels;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BlimpBot.Services
{
    public class CryptoRepository : ICryptoRepository
    {
        private readonly HttpClient _client;
        private readonly string _openExchangeRateApiToken;
        private readonly IConfiguration _configuration;
        public CryptoRepository(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            _openExchangeRateApiToken = _configuration["OpenExchangeRatesToken"];
        }

        public string GetChatResponse(List<string> argumentsList)
        {
            if (argumentsList.Count == 0)
                argumentsList = new List<string>() {"bitcoin", "dogecoin", "bananos", "fantom"};
            
            var response = GetCryptoRate(string.Join(",", argumentsList));
            var timestamp = DateTime.Now;

            var outString = $"Current value of cryptos in AUD as of {timestamp:dddd, dd MMMM yyyy HH:mm:ss}(AWST)\n";
            foreach ((string key, Dictionary<string, float> valueDict) in response)
            {
                outString += $"{key}: ${valueDict["aud"]}\n";
            }

            return outString;
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
