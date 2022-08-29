using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using static MongoDBConsoleApp.Program;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73488027/how-to-indexkeysdefinitionbuilder-change-to-indexkeysdefinition-mongodb-in-c/73508124#73508124">
    /// Question
    /// </a>
    /// </summary>
    internal class Solution_037 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Box> _collection = _db.GetCollection<Box>("box");

            dynamic setting = new
            {
                IsDescending = false,
                Column = "warehouseId",
                IsUnique = false
            };

            Console.WriteLine(CreateIndex(_collection, setting));
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private string CreateIndex<T>(IMongoCollection<T> _collection, dynamic setting)
            where T : class
        {
            IndexKeysDefinition<T> index;
            var indexBuilder = Builders<T>.IndexKeys;

            if (setting.IsDescending)
                index = indexBuilder.Descending(setting.Column);
            else
                index = indexBuilder.Ascending(setting.Column);

            var indexOptions = new CreateIndexOptions();
            if (setting.IsUnique)
                indexOptions.Unique = true;

            var model = new CreateIndexModel<T>(index, indexOptions);
            return _collection.Indexes.CreateOne(model);
        }

        class Box
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public string Content { get; set; }
            public string WarehouseId { get; set; }
        }
    }
}
