using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlimpBot.Interfaces;

namespace BlimpBot
{
    public class WeatherServices : IWeatherServices
    {
        public string GetWeatherString()
        {
            return @"Its cold up here :(... <i>Stupid Weather Blimp.</i>";
        }
    }
}
