using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using BlimpBot.Interfaces;
using BlimpBot.Types.OpenExchangeRatesTypes;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BlimpBot.Services
{
    public class OpenExchangeRateServices : IExchangeRateServices
    {
        private readonly HttpClient _client;
        private readonly string _openExchangeRateApiToken;
        private readonly IConfiguration _configuration;
        public OpenExchangeRateServices(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            _openExchangeRateApiToken = _configuration["OpenExchangeRatesToken"];
        }

        public string GetExchangeRateString(List<string> argumentsList)
        {
            var response = GetExchangeRates();
            var rates = response.rates;
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(response.timestamp).AddHours(8).DateTime;

            var outString = $"AUD Exchange Rates as of {timestamp:dddd, dd MMMM yyyy HH:mm:ss}(AWST)\n";
            foreach ((string key, float value) in rates)
            {
                if (key == "AUD")
                    outString += $"USD: {value}\n";
                else
                    outString += $"{key}: {value}\n";
            }

            return outString;
        }

        //Free tier only offers USD, so we need to convert to AUD
        private CurrencyResponse GetExchangeRates(string inCurrency = "USD", string outCurrencies = "AUD,CHF,EUR,GBP")
        {
            var openExchangeRateUrl = "https://openexchangerates.org/api/latest.json";
            var query = new Dictionary<string, string>
            {
                ["app_id"] = _openExchangeRateApiToken,
                ["symbols"] = outCurrencies
            };
            var request = QueryHelpers.AddQueryString(openExchangeRateUrl, query);
            var currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(_client.GetStringAsync(request).Result);

            //Convert to AUD
            var aud = currencyResponse.rates["AUD"];
            currencyResponse.rates["AUD"] /= aud; //we get to 1, then we get to 1/AUD i.e. USD in AUD
            foreach (var key in currencyResponse.rates.Keys.ToList())
            {
                currencyResponse.rates[key] /= aud;
            }
            return currencyResponse;

        }
    }
}
