using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
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
            settings.LinqProvider = MongoDB.Driver.Linq.LinqProvider.V3;

            MongoClient _client = new MongoClient(settings);

            ISolution solution = new Solution_061();
            await solution.RunAsync(_client);

            Console.ReadLine();
        }

        public static class Helpers
        {
            public static void RegisterCamelCasePack()
            {
                var pack = new ConventionPack();
                pack.Add(new CamelCaseElementNameConvention());
                ConventionRegistry.Register("camel case", pack, t => true);
            }
        }
    }
}
