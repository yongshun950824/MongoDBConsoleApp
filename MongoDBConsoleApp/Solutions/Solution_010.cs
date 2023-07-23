using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70729292/filter-ltex-x-price-9-getting-wrong-results/70729526#70729526">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_010 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var collection = _database.GetCollection<Product>("product");

            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Empty;
            filterDefinition &= new BsonDocument("$expr",
                new BsonDocument("$lte",
                    new BsonArray
                    {
                        new BsonDocument("$toInt", "$price"),
                        9
                    }));

            var result = collection.Find(filterDefinition)
                .ToList();

            Helpers.PrintFormattedJson(result);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        class Product
        {
            public ObjectId Id { get; set; }
            public string name { get; set; }
            public string price { get; set; }
        }
    }
}
