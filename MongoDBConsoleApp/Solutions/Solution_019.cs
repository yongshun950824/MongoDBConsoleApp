using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/71248390/not-equal-to-operator-for-mongodb-query-in-c-sharp/71248995#71248995">
    /// Question.
    /// </a>
    /// <br />
    /// <a href="https://stackoverflow.com/questions/71421359/c-sharp-mongodb-driver-convert-string-to-datetime-and-for-filter-builder/71422269#71422269">
    /// Related question.
    /// </a>
    /// </summary>
    internal class Solution_019 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            var _collection = _db.GetCollection<BsonDocument>("Student");

            FilterDefinition<BsonDocument> filter = new BsonDocument("$expr",
                new BsonDocument("$ne",
                    new BsonArray { "$section", "$upperSection" }
                )
            );

            var result = _collection.Find(filter).ToList();
            var count = _collection.Find(filter).CountDocuments();

            PrintOutput(result, count);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        private void PrintOutput(List<BsonDocument> result, long count)
        {
            Helpers.PrintFormattedJson(result);

            Console.WriteLine(count);
        }
    }
}
