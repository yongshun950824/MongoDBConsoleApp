using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/74065603/mongodb-net-driver-update-item-in-set/74065904#74065904">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_046 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<OrderBookEntity> _collection = _db.GetCollection<OrderBookEntity>("orderBook");

            var filter = new BsonDocument
            {
                { "Orders._id", "a611ffb1-c3e7-43d6-8238-14e311122125" }
            };
            var update = Builders<OrderBookEntity>.Update.Set("Orders.$.Amount", "100");

            UpdateResult result = await _collection.UpdateOneAsync(filter, update);

            Helpers.PrintFormattedJson(result);
        }

        class OrderBookEntity
        {
            [BsonId]
            public AssetDefinition UnderlyingAsset { get; set; }

            public List<OrderEntity> Orders { get; set; }
        }

        class AssetDefinition
        {
            public int Class { get; set; }
            public string Symbol { get; set; }
        }

        [BsonNoId]
        class OrderEntity
        {
            [BsonElement("_id")]
            public Guid Id { get; set; }

            public string Price { get; set; }

            public string Amount { get; set; }

            public int OrderAction { get; set; }

            public DateTime EffectiveTime { get; set; }
        }
    }
}
