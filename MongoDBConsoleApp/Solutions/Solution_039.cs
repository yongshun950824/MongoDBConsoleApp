using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MongoDBConsoleApp.Program;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73570202/mongodb-query-max-date-in-collection/73577781#73577781">
    /// Question
    /// </a>
    /// </summary>
    internal class Solution_039 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Order> _collection = _db.GetCollection<Order>("orders");

            var result = await GetAggregateGroupWithBsonDocument(_collection)
                .ToListAsync();

            PrintOutput(result);
        }

        private void PrintOutput(dynamic result)
        {
            if (result is List<BsonDocument>)
                Console.WriteLine(((List<BsonDocument>)result).ToJson(new JsonWriterSettings
                {
                    Indent = true
                }));
            else
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <param name="_collection"></param>
        /// <returns></returns>
        private IAggregateFluent<BsonDocument> GetAggregateGroupWithBsonDocument(IMongoCollection<Order> _collection)
        {
            BsonDocument groupStage = new BsonDocument
            {
                { "_id", 1 },
                { "regt", new BsonDocument("$max", "$registeredtime") }
            };

            return _collection.Aggregate()
                .Group<BsonDocument>(groupStage);
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <param name="_collection"></param>
        /// <returns></returns>
        private IAggregateFluent<OrderGroup> GetAggregateGroupWithExpression(IMongoCollection<Order> _collection)
        {
            return _collection.Aggregate()
                .Group<Order, int, OrderGroup>(x => 1, x => new OrderGroup
                {
                    Id = x.Key,
                    Regt = x.Max(y => y.Registeredtime)
                });
        }

        class Order
        {
            [BsonId]
            public ObjectId Id { get; set; }

            public DateTime Registeredtime { get; set; }
        }

        class OrderGroup
        {
            public int Id { get; set; }

            public DateTime Regt { get; set; }
        }
    }
}
