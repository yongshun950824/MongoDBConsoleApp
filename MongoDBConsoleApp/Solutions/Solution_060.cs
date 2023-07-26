using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/75159919/increment-all-fields-of-a-mongodb-document-from-c-sharp/75167534#75167534">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_060 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            int playerId = 10000;
            Stats stats = new Stats
            {
                Miss = 10,
                Success = 20,
                Failed = 5
            };

            BsonDocument updateFields = stats.ToBsonDocument();

            var filter = Builders<BsonDocument>.Filter.Eq("PlayerId", playerId);
            var update = new BsonDocument("$inc", updateFields);

            Helpers.PrintFormattedJson(update);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        class Stats
        {
            [BsonElement("Miss")]
            public uint Miss { get; set; }

            [BsonElement("Success")]
            public uint Success { get; set; }

            [BsonElement("Failed")]
            public uint Failed { get; set; }
        }
    }
}
