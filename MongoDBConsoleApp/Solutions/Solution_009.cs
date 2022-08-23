using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70702726/no-array-filter-found-for-identifier-1/70705559#70705559">
    /// Question
    /// </a>
    /// </summary>
    class Solution_009 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var collection = _database.GetCollection<BsonDocument>("people");

            var filter1 = Builders<BsonDocument>.Filter.Eq("_id", 1);
            var filter2 = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>("Friends"
                , new BsonDocument() { { "_id", 2 } });

            var obj = new
            {
                _id = 2,
                FirstName = "Bobby",
                LastName = "Marley",
                Gender = "Female"
            };

            var arrayFilters = new[]
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(
                    new BsonDocument("friend._id", 2)
                )
            };

            var update = Builders<BsonDocument>.Update.Set("Friends.$[friend]", obj.ToBsonDocument());

            var result = await collection.UpdateOneAsync(filter1 & filter2, update,
                options: new UpdateOptions { ArrayFilters = arrayFilters });

            PrintOutput(result);
        }

        private void PrintOutput(UpdateResult result)
        {
            Console.WriteLine(result);
            Console.WriteLine("Match: " + result.MatchedCount);
            Console.WriteLine("Modified: " + result.ModifiedCount);
        }
    }
}
