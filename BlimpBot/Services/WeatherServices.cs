using System.Collections.Generic;
using BlimpBot.Interfaces;

namespace BlimpBot
{
    public class WeatherServices : IWeatherServices
    {
        public string GetWeatherString(List<string> argumentsList)
        {
            return @"Its cold up here :(... <i>Stupid Weather Blimp.</i>";
        }
    }
}
