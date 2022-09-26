using System.Collections.Generic;
using BlimpBot.Interfaces;
using BlimpBot.Models;

namespace BlimpBot.Services
{
    public class WeatherServices : IWeatherServices
    {
        public OurChatResponse GetChatResponse(List<string> argumentsList)
        {
            return new OurChatResponse{Text = @"Its cold up here :(... <i>Stupid Weather Blimp.</i>"};
        }

    }
}
