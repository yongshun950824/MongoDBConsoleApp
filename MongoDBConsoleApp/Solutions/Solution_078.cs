using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/77484005/dotnet-api-error-type-system-text-json-jsonelement-is-not-configured-as-a-type-t/77484807#77484807">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_078 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            string json = @"{
              ""Testdata"": [
                {
                  ""data1"": ""name"",
                  ""data2"": ""textfield"",
                  ""data3"": true
                },
                {
                  ""data1"": ""email"",
                  ""data3"": ""email"",
                  ""data4"": true
                },
                {
                  ""randomData"": ""address"",
                  ""newData"": false,
                  ""anyData"": ""textarea""
                }
              ]
            }";

            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<MyModel> _collection = _database.GetCollection<MyModel>("posts");

            MyModel document = JsonSerializer.Deserialize<MyModel>(json);
            string serializedJson = JsonSerializer.Serialize(document, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            document = BsonSerializer.Deserialize<MyModel>(serializedJson);
            await _collection.InsertOneAsync(document);

            Console.WriteLine("After insert");
            Console.WriteLine(_collection.CountDocuments(new BsonDocument()));
        }

        public class MyModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string? Id { get; set; }

            public Object[] Testdata { get; set; }
        }
    }
}
