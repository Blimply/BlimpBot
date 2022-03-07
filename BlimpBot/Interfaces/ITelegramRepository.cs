using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlimpBot.Interfaces
{
    public interface ITelegramRepository
    {
        Task<ActionResult<string>> SendMessage(string message, long chatId);
        ActionResult<int> GetChatMemberCount(long chatId);
        Task<ActionResult<string>> GetBasicInfo();
    }
}
