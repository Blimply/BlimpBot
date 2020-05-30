using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlimpBot.Types.OpenExchangeRatesTypes
{
    public class CurrencyResponse
    {
        //someFunctionName(
        //{
        //    disclaimer: "https://openexchangerates.org/terms/",
        //    license: "https://openexchangerates.org/license/",
        //    timestamp: 1424127600,
        //    base: "CAD",
        //    rates:
        //    {
        //        AUD: 1.032828,
        //        EUR: 0.706867,
        //        GBP: 0.522328,
        //    }
        //}
        //)
        public string disclaimer;
        public string license;
        public int timestamp; //FIXME - proper timestamp type
        public Dictionary<string, float> rates;
    }
}
