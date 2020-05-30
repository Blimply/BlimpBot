using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlimpBot.Enums;

namespace BlimpBot.Interfaces
{
    public interface IMessageParser
    {
        string GetResponse(string message);
    }
}
