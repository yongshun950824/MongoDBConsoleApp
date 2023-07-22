using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70242147/mongodb-net-driver-pagination-on-array-stored-in-a-document-field/70242430#70242430">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_006 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var collection = _database.GetCollection<BsonDocument>("person");

            var id = 1;

            var bsonSearchParams = new BsonDocument
            {
                 new BsonElement( "_id" , id)
            };

            var bsonProjection = Builders<BsonDocument>.Projection
                .Slice("animals", 0, 3);

            var result = collection.Find(bsonSearchParams)
                .Project(bsonProjection)
                .FirstOrDefault();

            PrintOutput(result);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        private void PrintOutput(dynamic result)
        {
            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }
    }
}
