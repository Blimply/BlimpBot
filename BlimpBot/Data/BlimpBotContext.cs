using BlimpBot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlimpBot.Data
{
    public class BlimpBotContext : DbContext
    {
        public BlimpBotContext(DbContextOptions<BlimpBotContext> options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>().ToTable("Chat");
        }
    }
}
