using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73517217/mongodb-how-to-update-a-single-object-in-the-array-of-objects-inside-a-documen">
    /// Question
    /// </a>
    /// </summary>
    internal class Solution_038 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            string Code = "ABC40";

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Agencies> col = _db.GetCollection<Agencies>(Agencies.DocumentName);

            FilterDefinition<Agencies> filter = Builders<Agencies>.Filter.Eq("AgencyUsers.Code", Code);
            UpdateDefinition<Agencies> update = Builders<Agencies>.Update
                .Inc("AgencyUsers.$[agencyUser].TotalDownloads", 1);

            UpdateOptions updateOptions = new UpdateOptions
            {
                ArrayFilters = new[]
                {
                    new BsonDocumentArrayFilterDefinition<Agencies>(
                        new BsonDocument("agencyUser.Code", Code)
                    )
                }
            };
            UpdateResult result = await col.UpdateOneAsync(filter, update, updateOptions);

            Helpers.PrintFormattedJson(result);
        }

        class Agencies
        {
            [BsonIgnore]
            public const string DocumentName = "Agencies";

            [BsonId]
            public ObjectId _Id { get; set; }

            [BsonElement("name")]
            public string Name { get; set; }

            [BsonElement("id")]
            public string Id { get; set; }

            public AgencyUser[] AgencyUsers { get; set; }
        }

        class AgencyUser
        {
            public string UserName { get; set; }
            public string Code { get; set; }
            public string Link { get; set; }
            public int TotalDownloads { get; set; }
        }
    }
}
