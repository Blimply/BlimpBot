﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlimpBot.Interfaces
{
    public interface IExchangeRateServices
    {
        string GetExchangeRateString(List<string> argumentsList);
    }
}
