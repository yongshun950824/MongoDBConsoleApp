using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a hre"https://stackoverflow.com/questions/73591964/c-mongodb-driver-count-and-average-on-lookup-field/73592593#73592593">
    /// Question
    /// </a>
    /// </summary>
    internal class Solution_041 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            BsonClassMap.RegisterClassMap<ShopItem>(
                map =>
                {
                    map.AutoMap();
                    map.MapProperty(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
                });

            BsonClassMap.RegisterClassMap<RatingModel>(
                map =>
                {
                    map.AutoMap();
                    map.MapProperty(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
                    map.MapProperty(x => x.ShopItemId).SetSerializer(new GuidSerializer(BsonType.String));
                });

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<ShopItem> itemsCollection = _db.GetCollection<ShopItem>("ShopItem");

            var filterBuilder = Builders<ShopItem>.Filter;

            FilterDefinition<ShopItem> filter = filterBuilder.Empty;

            //if (filterOptions.NameToMatch != null)
            //{
            //    var nameFilter = filterBuilder.Where(item => item.Name.Contains(filterOptions.NameToMatch));
            //    filter &= nameFilter;
            //}
            //if (filterOptions.SeasonToMatch != null)
            //{
            //    var seasonFilter = filterBuilder.Where(item => item.Season == filterOptions.SeasonToMatch);
            //    filter &= seasonFilter;
            //}

            ProjectionDefinition<BsonDocument> projection = new BsonDocument
            {
                {
                    "AmountOfRatings",
                    new BsonDocument("$size", "$Ratings")
                },
                {
                    "AverageRating",
                    new BsonDocument("$avg", "$Ratings.Rate")
                }
            };

            var items = await itemsCollection
                .Aggregate()
                .Match(filter)
                .Lookup("Rate", "_id", "ShopItemId", "Ratings") // BsonDocument After that line
                .Project(projection)
                .ToListAsync();

            Console.WriteLine(items.ToJson(new MongoDB.Bson.IO.JsonWriterSettings
            {
                Indent = true
            }));
        }

        public class ShopItem
        { //item structure
            [BsonId]
            public Guid Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public decimal Price { get; set; }

            public string Description { get; set; } = string.Empty;

            //public SeasonEnum Season { get; set; }

            public DateTimeOffset CreatedDate { get; set; }
        }

        public class RatingModel
        {
            [BsonId]
            public Guid Id { get; set; }

            public Guid UserId { get; set; }

            public Guid ShopItemId { get; set; }

            [Range(1, 5)]
            public Int16 Rate { get; set; }

            public DateTimeOffset CreatedDate { get; set; }
        }
    }
}
