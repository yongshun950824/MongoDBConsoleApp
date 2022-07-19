using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/71541098/c-sharp-and-mongodb-include-or-exclude-element-of-array-using-projection/71543925#71543925">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_021 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            var collection = _db.GetCollection<BsonDocument>("user");

            var filter = Builders<BsonDocument>.Filter.Empty;
            var projection = Builders<BsonDocument>.Projection
                .Include("email")
                .Include("addresses.label")
                .Include("addresses.city")
                .Include("addresses.country")
                .Exclude("_id");

            var result = collection.Find(filter)
                .Project(projection)
                .ToList();

            PrintOutput(result);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private void PrintOutput(List<BsonDocument> result)
        {
            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }
    }
}
