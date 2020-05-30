using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlimpBot.Types.OpenExchangeRatesTypes;

namespace BlimpBot.Interfaces
{
    public interface IExchangeRateServices
    {
        string GetExchangeRateString();
        CurrencyResponse GetExchangeRates(string inCurrency, string outCurrencies);
    }
}
