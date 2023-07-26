using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    internal class Solution_057 : ISolution
    {
        /// <summary>
        /// <a href="https://stackoverflow.com/questions/75000121/cannot-dynamically-create-an-instance-of-type-system-text-json-nodes-jsonobject/75000361#75000361">
        /// Question.
        /// </a>
        /// </summary>
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            JsonObject item = JsonNode.Parse(@"
            {
                ""AppId"":""(an already existing app id)"",
                ""Fields"":
                {
                    ""NombreCliente"": ""Pepillo"",
                    ""Email"": ""pepiko@email.com"",
                    ""Telefono"": ""pp56656784"",
                    ""Localidad"": ""Pepeland"",
                    ""Facturas"": [""848435498"",""0564864984""]
                }
            }").AsObject();

            await InsertItem(_client, item);

            List<Item> result = await GetAllAppItems(_client, "");

            Helpers.PrintFormattedJson(result);
        }

        private async Task InsertItem(IMongoClient _client, JsonObject item)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            var jsonString = JsonSerializer.Serialize(item, options);

            var document = BsonSerializer.Deserialize<BsonDocument>(jsonString);

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<BsonDocument> collection2 = _db.GetCollection<BsonDocument>("Items");
            await collection2.InsertOneAsync(document);
        }

        private async Task<List<Item>> GetAllAppItems(IMongoClient _client, string appId)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Item> _collection = _db.GetCollection<Item>("Items");

            return await _collection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        class Item
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public string AppId { get; set; }

            public dynamic Fields { get; set; }
        }
    }
}
