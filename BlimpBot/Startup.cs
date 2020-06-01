using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
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

            // Can't put this an app-setting since the port is non-default and may chang
            var connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
            
            if (connectionString == null || string.IsNullOrWhiteSpace(connectionString))
                connectionString = Configuration.GetConnectionString("BlimpBotContext");
            else
                connectionString = NormaliseAzureMySQLInAppConnString(connectionString);

            System.Diagnostics.Trace.TraceError("Connection string is:"+connectionString);
            services.AddDbContext<BlimpBotContext>(options => options.UseMySQL(connectionString));

            services.AddSingleton<HttpClient>();

            services.AddScoped<IWeatherServices, WeatherServices>();
            services.AddScoped<IMessageParser, MessageParserServices>();
            services.AddScoped<IExchangeRateServices, OpenExchangeRateServices>();
            services.AddScoped<ITelegramServices, TelegramServices>();
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

        //based on SO answer https://stackoverflow.com/a/58020841 by itminus
        private string NormaliseAzureMySQLInAppConnString(string connectionString)
        {
            connectionString += ";";
            //MySQL string format by default looks like:
            //Database=localdb;Data source=127.0.0.1:50356;User Id=azure;Password=randPassString

            var database = new Regex(@"Database=(.*?);").Match(connectionString).Groups[1];
            var userId = new Regex(@"User Id=(.*?);").Match(connectionString).Groups[1];
            var password = new Regex(@"Password=(.*?);").Match(connectionString).Groups[1];

            var dataSourceTmp = new Regex(@"Data Source=(.*?):(.*?);").Match(connectionString).Groups;
            var server = dataSourceTmp[1];
            var port = dataSourceTmp[2];
            //Convert to format
            //Server=127.0.0.01;Port=50356;Database=localdb;Uid=Azure;Pwd=randPassString
            return $"Server={server};Port={port};Database={database};Uid={userId};Pwd={password};";
        }

    }
}
