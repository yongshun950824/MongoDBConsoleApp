using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/69582406/query-with-filter-builder-on-nested-array-using-mongodb-c-sharp-driver-with-a-gi/69583877#69583877">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_003 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<Produit> _collection = _database.GetCollection<Produit>("produit");

            List<string> uids = new List<string> { "STK-00113", "STK-00117", "STK-00113", "STK-00114" };
            FilterDefinition<Produit> filter = new BsonDocument(
                "foobar.uid",
                new BsonDocument(
                    "$in",
                    BsonArray.Create(uids)
                )
            );
            var results = _collection.Find(filter).ToList();

            PrintOutput(results);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        private void PrintOutput(List<Produit> results)
        {
            Console.WriteLine(results.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }

        class Produit
        {
            [BsonId]
            public string Id { get; set; }
            public string uid { get; set; }
            public string type { get; set; }
            public List<FooBar> foobar { get; set; }
        }

        class FooBar
        {
            public string uid { get; set; }
            public string nom { get; set; }
        }
    }
}
