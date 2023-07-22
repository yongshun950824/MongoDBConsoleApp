using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/69403622/mongodb-net-driver-pull-multiple-elements-from-arrays-that-exist-in-multiple/69422853#69422853">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_002 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<Store> _collection = _database.GetCollection<Store>("store");

            InitData(_collection);

            string[] removedFruits = new string[] { "apples", "bananas" };

            Update(_collection, removedFruits);

            FilterDefinition<Store> filter = Builders<Store>.Filter.Empty;
            var items = _collection.Find(filter)
                .ToList<Store>();

            PrintOutput(items);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        private void InitData(IMongoCollection<Store> _collection)
        {
            _collection.InsertMany(new List<Store>
            {
                new Store { Name = "Store 1", Fruits = new string[] { "apples", "mangoes" } },
                new Store { Name = "Store 2", Fruits = new string[] { "apples", "bananas", "papayas" } }
            });

            Thread.Sleep(2000);
        }

        private void Update(IMongoCollection<Store> _collection, string[] removedFruits)
        {
            FilterDefinition<Store> filter = Builders<Store>.Filter.Empty;

            UpdateDefinition<Store> update =
                new BsonDocument("$pull",
                    new BsonDocument("fruits",
                        new BsonDocument("$in", BsonArray.Create(removedFruits))
                        )
                );

            _collection.UpdateMany(
                filter,
                update
            );
        }

        private void UpdateWithArrayFilters(IMongoCollection<Store> _collection, string[] removedFruits)
        {
            FilterDefinition<Store> filter = Builders<Store>.Filter.Empty;

            UpdateDefinition<Store> update = "{ $pull: { \"fruits\": { $in: [\"apples\", \"bananas\"] } } }";

            //Or
            //FilterDefinition<Store> updateIn = Builders<Store>.Filter.AnyIn("fruits", removedFruits);
            //UpdateDefinition<Store> update = Builders<Store>.Update.Pull("fruits", updateIn);

            var arrayFilters = new[]
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(
                    new BsonDocument("fruits",
                        new BsonDocument("$in", new BsonArray(removedFruits)))
                    )
            };

            _collection.UpdateMany(
                filter,
                update,
                new UpdateOptions { ArrayFilters = arrayFilters }
            );
        }

        private void PrintOutput(List<Store> items)
        {
            Console.WriteLine(items.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }

        class Store
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public string Name { get; set; }
            public string[] Fruits { get; set; }
        }
    }
}
