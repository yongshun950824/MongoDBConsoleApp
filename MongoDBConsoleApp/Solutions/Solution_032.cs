using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73298130/mongodb-net-driver-increment-a-value-inside-dictionary/73300031#73300031">
    /// Question
    /// </a>
    /// </summary>
    internal class Solution_032 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<OrderDetails> _collection = _db.GetCollection<OrderDetails>("OrderDetails");

            PrintBeforeOutput(_collection.Find(FilterDefinition<OrderDetails>.Empty)
                .ToList());

            int userId = 1;

            var update = Builders<OrderDetails>.Update
                .Inc($"totalViewsPerUser.{userId}", 1);

            UpdateResult updateResult = _collection.UpdateMany(
                FilterDefinition<OrderDetails>.Empty,
                update);

            PrintUpdateResultOutput(updateResult);

            PrintAfterOutput(_collection.Find(FilterDefinition<OrderDetails>.Empty)
                .ToList());
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        private void PrintBeforeOutput(List<OrderDetails> result)
        {
            Console.WriteLine("Before update:");
            PrintOutput(result);
            Console.WriteLine("---");
        }

        private void PrintUpdateResultOutput(UpdateResult result)
        {
            PrintOutput(result);
            Console.WriteLine("---");
        }

        private void PrintAfterOutput(List<OrderDetails> result)
        {
            Console.WriteLine("After update:");
            PrintOutput(result);
        }

        private void PrintOutput(dynamic result)
        {
            Helpers.PrintFormattedJson(result);
        }

        class OrderDetails
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public int OrderId { get; set; }
            public Dictionary<int, int> TotalViewsPerUser { get; set; }
        }
    }
}
