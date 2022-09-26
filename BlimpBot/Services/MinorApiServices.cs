using System;
using System.Collections.Generic;
using System.Net.Http;
using BlimpBot.Constants;
using BlimpBot.Interfaces;
using BlimpBot.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace BlimpBot.Services
{
    public class MinorApiServices : IMinorApiServices
    {
        private readonly HttpClient _client;
        public MinorApiServices(HttpClient client)
        {
            _client = client;
        }
        public OurChatResponse GetChatResponse(List<string> arguments, MinorApiType apiType)
        {
            return apiType switch
            {
                MinorApiType.None => new OurChatResponse(),
                MinorApiType.Duck => HandleDuckApi(),
                MinorApiType.Dog => HandleDogApi(),
                MinorApiType.Cat => HandleCatApi(),
                MinorApiType.Coffee => HandleCoffeeApi(),

                _ => new OurChatResponse()
            };
        }

        private record DuckResponseModel
        {
            //{"message":"Powered by random-d.uk","url":"https://random-d.uk/api/254.jpg"}
            public string message { get; init; }
            public string url { get; init; }
        }
        private OurChatResponse HandleDuckApi()
        {
            var baseUrl = "https://random-d.uk/api/v2/quack";
            var query = new Dictionary<string, string>();
            var request = QueryHelpers.AddQueryString(baseUrl, query);
            var response = JsonConvert.DeserializeObject<DuckResponseModel>(_client.GetStringAsync(request).Result);
            return new OurChatResponse
            {
                Text = "Quack! Source: random-d.uk",
                PhotoUrl = response.url,
                IsPhotoMessage = true,
            };
        }

        private record DogResponseModel
        {
            public string fileSizeBytes { get; init; }
            public string url { get; init; }
        }
        private OurChatResponse HandleDogApi()
        {
            //https://random.dog/woof.json
            //{"fileSizeBytes":340460,"url":"https://random.dog/1d3ca62c-4c19-4d6b-b9ef-87716b3eccf5.png"}
            var baseUrl = "https://random.dog/woof.json";
            var query = new Dictionary<string, string>();
            var request = QueryHelpers.AddQueryString(baseUrl, query);
            var response = JsonConvert.DeserializeObject<DogResponseModel>(_client.GetStringAsync(request).Result);
            return new OurChatResponse
            {
                Text = "Woof! Source: random.dog",
                PhotoUrl = response.url,
                IsPhotoMessage = true,
            };
        }

        public class CatResponseModel
        {
            public string id { get; set; }
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        private OurChatResponse HandleCatApi()
        {
            //https://api.thecatapi.com/v1/images/search
            //[{"id":"MjA2NjQzMw","url":"https://cdn2.thecatapi.com/images/MjA2NjQzMw.jpg","width":853,"height":1280}]
            var baseUrl = "https://api.thecatapi.com/v1/images/search";
            var query = new Dictionary<string, string>();
            var request = QueryHelpers.AddQueryString(baseUrl, query);
            var response = JsonConvert.DeserializeObject<List<CatResponseModel>>(_client.GetStringAsync(request).Result);
            return new OurChatResponse
            {
                Text = "Meow! Source: thecatapi.com",
                PhotoUrl = response[0].url,
                IsPhotoMessage = true,
            };
        }

        private record CoffeeResponseModel
        {
            public string file { get; init; }
        }
        private OurChatResponse HandleCoffeeApi()
        {
            // https://coffee.alexflipnote.dev/random.json
            //{"file": "https://coffee.alexflipnote.dev/7ZsqTFEFyYQ_coffee.png"}
            var baseUrl = "https://coffee.alexflipnote.dev/random.json";
            var query = new Dictionary<string, string>();
            var request = QueryHelpers.AddQueryString(baseUrl, query);
            var response = JsonConvert.DeserializeObject<CoffeeResponseModel>(_client.GetStringAsync(request).Result);
            return new OurChatResponse
            {
                Text = "Yum! Source: coffee.alexflipnote.dev",
                PhotoUrl = response.file,
                IsPhotoMessage = true,
            };
        }

        public OurChatResponse GetChatResponse(List<string> arguments)
        {
            return GetChatResponse(arguments, MinorApiType.None);
        }
    }
}