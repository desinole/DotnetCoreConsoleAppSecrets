using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace console
{
    class Program
    {
        static IConfiguration Configuration;
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            //mapping static variable to strongly typed class
            var appSecrets = Configuration.GetSection(nameof(MyAppSecrets)).Get<MyAppSecrets>();
            Console.WriteLine($"Strongly typed mapping {appSecrets.CloudSecrets.SQLConnectionString}");
            //access secrets using key value pair
            Console.WriteLine($"Key value pair {Configuration["MyAppSecrets:CloudSecrets:SQLConnectionString"]}");
            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseEnvironment("development")
            .ConfigureAppConfiguration((hostingContext, configuration) => {
                configuration.Sources.Clear();
                Configuration = configuration.AddUserSecrets<MyAppSecrets>().Build(); 
            });
    }
    public class MyAppSecrets{
        public AzureSecrets CloudSecrets{get;set;}
        public ExternalSecrets UtilitySecrets{get;set;}
    }
    public class AzureSecrets{
        public string SQLConnectionString{get;set;}
        public string CosmosConnectionString{get;set;}
    }

    public class ExternalSecrets{
        public string TwilioApiKey{get;set;}
        public string LaunchDarklyApiKey{get;set;}
    }
}