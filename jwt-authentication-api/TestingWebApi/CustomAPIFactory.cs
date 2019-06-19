using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using WebApi.Services;
using WebApi.Helpers;
using System.IO;

namespace TestingWebApi
{
    public class CustomAPIFactory<TStartup> : WebApplicationFactory<WebApi.Startup>
    {
        protected static readonly Dictionary<string, string> arrayDict = new Dictionary<string, string>
        {
            {"MongoDB:Database", "Test_DB"},
        };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //var projectDir = Directory.GetCurrentDirectory();
            //var configPath = Path.Combine(projectDir, "appsettings.json");
            //var configuration = new ConfigurationBuilder()
            //   .AddJsonFile("appsettings.json")
            //   .Build();

            // Add database name for testing
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddInMemoryCollection(arrayDict);

                //config.AddJsonFile(configPath);
            })
            .ConfigureServices(services =>
            {
                services.AddScoped<IUserService, UserService>();

                //services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            });
            
        }

    }

}
