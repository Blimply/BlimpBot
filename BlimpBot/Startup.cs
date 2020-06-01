using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BlimpBot.Data;
using BlimpBot.Interfaces;
using BlimpBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlimpBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddControllersWithViews()
                    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy =
                                                                        JsonNamingPolicy.CamelCase);

            //Default to a named conn string if it exists or use MySQL In App
            var connectionString = Configuration.GetConnectionString("BlimpBotContext");

            // Can't put this an app-setting since the port is non-default and may change
            if (connectionString == null || string.IsNullOrWhiteSpace(connectionString))
                Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");                                                                                                


            services.AddDbContext<BlimpBotContext>(options => options.UseMySQL(connectionString));

            services.AddSingleton<HttpClient>();

            services.AddSingleton<IWeatherServices, WeatherServices>();
            services.AddSingleton<IMessageParser, MessageParserServices>();
            services.AddSingleton<IExchangeRateServices, OpenExchangeRateServices>();
            services.AddSingleton<ITelegramServices, TelegramServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }

    }
}
