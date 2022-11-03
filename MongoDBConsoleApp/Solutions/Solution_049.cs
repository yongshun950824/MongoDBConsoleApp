using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/74285939/using-elemmatch-with-filterdefiniton-by-mongodb-drive/74286414#74286414">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_049 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            this.RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<MongoCollection> _col = _db.GetCollection<MongoCollection>("Solution_049");

            DateTime fromDate = new DateTime(2022, 08, 01);
            DateTime tillDate = new DateTime(2022, 11, 01);

            var filter = Builders<MongoCollection>.Filter.ElemMatch("field2",
              Builders<object>.Filter.And(
                Builders<object>.Filter.Gte("myDatetime.ts", fromDate),
                Builders<object>.Filter.Lte("myDatetime.ts", tillDate))
            );

            var result = (await _col.FindAsync(filter)).ToList();
            PrintOutput(result);
        }

        private void PrintOutput(List<MongoCollection> result)
        {
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }

    public class MongoCollection
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string ID { get; set; }
        public object field1 { get; set; }

        public List<object> field2 { get; set; }
    }
}
