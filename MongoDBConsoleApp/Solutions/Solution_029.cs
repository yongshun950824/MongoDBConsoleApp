using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73109463/find-lastly-added-collection-with-some-condition-in-mongodb-and-c-sharp/73109820#73109820">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_029 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<HotelResultDocument> _collection = _database.GetCollection<HotelResultDocument>("HotelResult");

            List<string> rateKeys = new List<string> { "1", "5" };
            var filter = Builders<HotelResultDocument>.Filter.In(x => x.RateKey, rateKeys);

            //filter &= Builders<HotelResultDocument>.Filter.Eq(x => x.RateType, "BOOKABLE");

            //filter &= Builders<HotelResultDocument>.Filter.Gt(x => x.ExpiredAt, DateTime.Now);

            SortDefinition<HotelResultDocument> sort =
                Builders<HotelResultDocument>.Sort.Descending(x => x.CreatedAt);

            ProjectionDefinition<HotelResultDocument, GroupedHotelResult> group =
                new BsonDocument
                {
                    { "_id", "$RateKey" },
                    { "hotel", new BsonDocument
                        {
                            { "$first", "$$ROOT" }
                        }
                    }
                };

            var result = await _collection
                .Aggregate()
                .Match(filter)
                .Sort(sort)
                .Group<GroupedHotelResult>(group)
                .ReplaceWith<GroupedHotelResult, HotelResultDocument>(x => x.Hotel)
                .ToListAsync();

            Helpers.PrintFormattedJson(result);
        }

        class GroupedHotelResult
        {
            [BsonId]
            public string Id { get; set; }

            [BsonElement("hotel")]
            public HotelResultDocument Hotel { get; set; }
        }

        class HotelResultDocument
        {
            [BsonId]
            public ObjectId Id { get; set; }

            [BsonIgnore]
            public string Reference { get; set; }

            [BsonIgnore]
            public string Token { get; set; }

            [BsonElement("hotelCode")]
            public int HotelCode { get; set; }

            public string RateKey { get; set; }

            [BsonIgnore]
            public string RateClass { get; set; }

            [BsonIgnore]
            public string RateType { get; set; }

            [BsonIgnore]
            public double Net { get; set; }

            public DateTime CreatedAt { get; set; }

            [BsonIgnore]
            public DateTime ExpiredAt { get; set; }
        }
    }
}
