using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/69983653/mongodb-driver-buildersdynamic-dont-work-on-equal-to-date/69984438#69984438">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_005 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("sample_mflix");
            var _collection = _database.GetCollection<dynamic>("movies");
            var query = _collection.AsQueryable();

            FilterDefinition<dynamic> searchFilter = FilterDefinition<dynamic>.Empty;
            string x = "lastupdated";
            string y = "2015-07-27";
            string z = "2015-07-28";
            //var SearchDAte = DateTime.Parse(y, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            //searchFilter &= Builders<BsonDocument>.Filter.Eq(x, SearchDAte);

            var startDate = DateTime.Parse(y, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var endDate = DateTime.Parse(z, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            //searchFilter &= Builders<dynamic>.Filter.Gte(x, startDate);
            //searchFilter &= Builders<dynamic>.Filter.Lt(x, endDate);

            BsonDocument filterDoc = new BsonDocument("$expr",
                new BsonDocument("$eq",
                    new BsonArray
                    {
                        new BsonDocument("$dateToString",
                            new BsonDocument
                            {
                                { "format", "%Y-%m-%d" },
                                { "date", "$lastupdated" }
                            }),
                        "2015-07-27"
                    }
                )
            );

            searchFilter &= filterDoc;

            var results = _collection.Find(searchFilter)
                .ToList();

            PrintOutput(results);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private void PrintOutput(List<dynamic> results)
        {
            foreach (var movie in results)
            {
                Console.WriteLine("Title: {0}, Last Updated: {1}"
                    , movie["title"].ToString()
                    , Convert.ToDateTime(movie["lastupdated"]).ToString("yyyy-MM-dd HH:mm:ss"));
            }

            Console.WriteLine("Count: {0}", results.Count);
        }
    }
}
