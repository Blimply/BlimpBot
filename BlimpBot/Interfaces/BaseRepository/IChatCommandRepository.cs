using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlimpBot.Constants;
using BlimpBot.Models;

namespace BlimpBot.Interfaces
{
    public interface IChatCommandRepository
    {
        virtual OurChatResponse GetChatResponse(List<string> arguments, MinorApiType minorApiType)
        {
            return GetChatResponse(arguments);
        }

        OurChatResponse GetChatResponse(List<string> arguments);

    }
}
