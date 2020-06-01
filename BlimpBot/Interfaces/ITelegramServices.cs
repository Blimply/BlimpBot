using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlimpBot.Interfaces
{
    public interface ITelegramServices
    {
        Task<ActionResult<string>> SendMessage(string message, int chatId);
        Task<ActionResult<int>> GetChatMemberCount(int chatId);
        Task<ActionResult<string>> GetBasicInfo();
    }
}
