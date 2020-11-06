using BlimpBot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BlimpBot.Database
{
    public class BlimpBotContext : DbContext
    {
        public BlimpBotContext(DbContextOptions<BlimpBotContext> options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                        .Property(p => p.Emoji)
                        .IsUnicode();
        }
    }
}
