using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlimpBot.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace BlimpBot.Data
{
    public static class DbInitialiser
    {
        public static void Initialise(BlimpBotContext context)
        {
            context.Database.Migrate();

            if (context.Chats.Any())
                return;
            var now = DateTime.Now; //sometimes databases get messy with DateTime calls

            var chats = new Chat[]
            {
                new Chat
                {
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
