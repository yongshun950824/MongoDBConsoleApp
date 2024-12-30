using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using MongoDBConsoleApp.Solutions;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace MongoDBConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var mongoUri = ConfigurationManager.ConnectionStrings["MongoUri"].ToString();

            MongoClientSettings settings = MongoClientSettings.FromConnectionString(
                mongoUri
            );

            if (GetAppSettings<bool>("TraceMongoEvent"))
            {
                // Query Interceptor: https://stackoverflow.com/q/48947260/8017690
                settings.ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                    {
                        Console.WriteLine(e.CommandName);
                        Helpers.PrintFormattedJson(e.Command);
                    });
                };
            }

            MongoClient _client = new MongoClient(settings);

            ISolution solution = new Solution_086();
            await solution.RunAsync(_client);

            Console.ReadLine();
        }

        static T GetAppSettings<T>(string key)
        {
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                return default;

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}
