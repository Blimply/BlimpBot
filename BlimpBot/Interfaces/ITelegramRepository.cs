﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlimpBot.Interfaces
{
    public interface ITelegramRepository
    {
        Task<ActionResult<string>> SendMessage(string message, int chatId);
        ActionResult<int> GetChatMemberCount(int chatId);
        Task<ActionResult<string>> GetBasicInfo();
    }
}
