using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Microsoft.Extensions.Configuration;

namespace BlimpBot.Pages
{
    public class HomePageModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public HomePageModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        //Nothing here yet!
    }
}