using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    internal class Solution_013 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var collection = _database.GetCollection<BsonDocument>("person");

            string name = "rob";
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match",
                    new BsonDocument
                    {
                        { "name", BsonRegularExpression.Create(new Regex(name, RegexOptions.IgnoreCase)) }
                    }
                )
            };

            var result = collection
                .Aggregate<BsonDocument>(pipeline)
                .ToList();

            Helpers.PrintFormattedJson(result);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }
    }
}
