using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp
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

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        void PrintOutput(List<Produit> results)
        {
            foreach (var item in results)
            {
                Console.WriteLine(item.Id);
            }
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
