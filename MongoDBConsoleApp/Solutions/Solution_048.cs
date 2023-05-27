using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/74214246/mongodb-get-all-elements-inside-a-bsonarray-and-convert-into-liststring/74215348#74215348">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_048 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>("types");

            var filter = Builders<BsonDocument>.Filter
                .Exists("Types", true);
            var projection = Builders<BsonDocument>.Projection
                .Include("Types")
                .Exclude("_id");
            var result = collection.Find(filter)
                .Project(projection)
                .First();

            List<string> types = ((BsonArray)result["Types"]).Values
                .Select(x => x.AsString)
                .ToList();

            Console.WriteLine(String.Join(",", types));
        }

        public Task RunAsync(IMongoClient _client)
        {
            Run(_client);
            return Task.CompletedTask;
        }
    }
}
