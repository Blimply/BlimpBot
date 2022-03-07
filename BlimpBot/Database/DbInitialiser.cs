using System;
using System.Linq;
using BlimpBot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BlimpBot.Database
{
    public static class DbInitialiser
    {
        public static void Initialise(BlimpBotContext context)
        {
            context.Database.Migrate();

            if (context.Chats.Any())
                return;
            var now = DateTime.Now; //sometimes databases get messy with DateTime calls

            var chats = new[]
            {
                new Chat
                {
                    ChatId = "0",
                    MembersCount = 0,
                    Name = "Seed data",
                    LastMessageReceived = now
                }
            };

            context.Chats.AddRange(chats);
            context.SaveChanges();
        }
    }
}
