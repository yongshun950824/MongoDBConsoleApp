using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/78283250/mongodb-issues-with-translating-js-query-to-c-sharp/78283453#78283453">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_080 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<SirenRepresentation> sirens = _database.GetCollection<SirenRepresentation>("sirens");

            long userId = 99999998;

            var query = sirens.AsQueryable()
                .Where(_siren => _siren.OwnerId == userId
                    || (_siren.Listener != null
                        && (_siren.Listener ?? new long[] { }).Any(x => x == userId))
                    || (_siren.Responsible != null
                        && (_siren.Responsible ?? new long[] { }).Any(x => x == userId)))
                .GroupBy(s => true)
                .Select(g => new UserStatistics
                {
                    SirenasCount = g.Sum(_siren => _siren.OwnerId == userId ? 1 : 0),
                    Subscriptions = g.Sum(_siren => (_siren.Listener != null
                        && (_siren.Listener ?? new long[] { }).Contains(userId)) ? 1 : 0),
                    Responsible = g.Sum(_siren => (_siren.Responsible != null
                        && (_siren.Responsible ?? new long[] { }).Contains(userId)) ? 1 : 0)
                });

            Helpers.PrintFormattedJson(query.ToList());
        }
    }

    [BsonNoId]
    internal class UserStatistics
    {
        public int SirenasCount { get; set; }
        public int Subscriptions { get; set; }
        public int Responsible { get; set; }
    }

    internal class SirenRepresentation
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("ownerid"), BsonRepresentation(BsonType.Int64)]
        public long OwnerId { get; set; }

        [BsonRepresentation(BsonType.Int64)]
        [BsonElement("listener")]
        public long[] Listener { get; set; } = new long[] { };

        [BsonRepresentation(BsonType.Int64)]
        [BsonElement("responsible")]
        public long[] Responsible { get; set; } = new long[] { };
    }
}
