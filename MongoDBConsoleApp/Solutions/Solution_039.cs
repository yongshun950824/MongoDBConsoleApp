using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Order> _collection = _db.GetCollection<Order>("orders");

            #region Solution 1
            var result = await GetAggregateGroupWithBsonDocument(_collection)
                .ToListAsync();
            #endregion

            #region Solution 2
            //var result = await GetAggregateGroupWithExpression(_collection)
            //    .ToListAsync();
            #endregion

            Helpers.PrintFormattedJson(result);
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
