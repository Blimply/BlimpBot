using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlimpBot.Interfaces
{
    public interface IWeatherRepository
    {
        string GetWeatherString(List<string> argumentsList);
    }
}
