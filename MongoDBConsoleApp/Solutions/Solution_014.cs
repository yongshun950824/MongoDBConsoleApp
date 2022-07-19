using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70765835/mongodb-net-driver-query-for-lte-and-gte-throwing-error-an-object-representin/70767786#70767786">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_014 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var collection = _database.GetCollection<BsonDocument>("visitTask");

            var rating = 3.0m;
            var cookTime = "5";

            FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Empty;

            filterDefinition &= Builders<BsonDocument>.Filter.Eq("ContentTypeId", 2);

            filterDefinition &=
                new BsonDocument("$gte",
                   new BsonArray 
                   {
                       new BsonDocument("$toDouble", "$ContentAverageRating"),
                       Convert.ToDouble(rating)
                   }
                );

            filterDefinition &=
                new BsonDocument("$lte",
                   new BsonArray 
                   {
                       new BsonDocument("$toDecimal", "$ContentTime"),
                       Decimal.Parse(cookTime)
                   }
                );

            FilterDefinition<BsonDocument> rootFilterDefinition = new BsonDocument("$expr",
                filterDefinition.ToBsonDocument());

            collection
                .Aggregate()
                .Match(rootFilterDefinition)
                .ToList();

            Console.WriteLine(rootFilterDefinition.ToBsonDocument().ToJson());
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }
    }
}
