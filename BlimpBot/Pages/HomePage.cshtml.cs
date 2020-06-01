using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlimpBot.Data;
using BlimpBot.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlimpBot.Pages
{
    public class HomePageModel : PageModel
    {
        private readonly BlimpBotContext _context;
        
        public List<Chat> Chats { get; set; }

        public HomePageModel(BlimpBotContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            Chats = await _context.Chats.ToListAsync();
            return Page();
        }
    }
}