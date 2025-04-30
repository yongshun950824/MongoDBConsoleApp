using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/79568837/backward-compatible-mongo-fields-using-c-sharp-and-net/79568869#79568869">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_088 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<Profile> _collection = _database.GetCollection<Profile>("profile");

            await _collection.InsertOneAsync(new Profile
            {
                FullName = "test"
            });
        }

        public class Profile
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; } = null!;

            // Deprecate
            [Obsolete]
            [BsonElement("FirstName")]
            [BsonIgnoreIfNull]
            [JsonPropertyName("FirstName")]
            public string? FirstName { get; set; }

            // Deprecate
            [Obsolete]
            [BsonElement("LastName")]
            [BsonIgnoreIfNull]
            [JsonPropertyName("LastName")]
            public string? LastName { get; set; }

            // New field
            [BsonElement("FullName")]
            [JsonPropertyName("FullName")]
            public string? FullName { get; set; }
        }
    }
}
