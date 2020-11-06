using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlimpBot.Database;

namespace BlimpBot.Repository
{
    public class BaseRepository
    {
        internal BlimpBotContext Context;
        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
