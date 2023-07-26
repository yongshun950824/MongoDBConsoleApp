using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/74570841/mongodb-net-driver-using-pullfilter-to-remove-string-from-string-array/74587442#74587442">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_051 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Descriptor> _Collection = _db.GetCollection<Descriptor>("descriptor");

            ObjectId id = new ObjectId("6382de8309a177288bd3cf27");
            string actioner = "user1";

            UpdateResult result = await _Collection.UpdateOneAsync(
                Builders<Descriptor>.Filter.Eq(d => d.Id, id),
                Builders<Descriptor>.Update
                    .Set(d => d.UpdatedBy, actioner)
                    .Set(d => d.UpdatedOn, DateTime.Now)
                    .Pull(d => d.Options, "Remove this one")
            );

            Helpers.PrintFormattedJson(result);
        }

        class Descriptor
        {
            [BsonId]
            public ObjectId Id { get; set; }

            public string Name { get; set; }

            public int Type { get; set; }

            public string[] Options { get; set; }

            public string UpdatedBy { get; set; }

            public DateTime UpdatedOn { get; set; }
        }
    }
}
