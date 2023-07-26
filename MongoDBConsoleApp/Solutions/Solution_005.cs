using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
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

            FilterDefinition<dynamic> searchFilter = FilterDefinition<dynamic>.Empty;
            string x = "lastupdated";
            string y = "2015-07-27";
            string z = "2015-07-28";

            #region Solution 1
            //searchFilter = GetFilterDefinitionWithFluent(x, y, z);
            #endregion

            #region Solution 2
            searchFilter = GetFilterDefinitionWithBsonDocument(y);
            #endregion

            var result = _collection.Find(searchFilter)
                .ToList();

            PrintOutput(result);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        private FilterDefinition<dynamic> GetFilterDefinitionWithFluent(string x, string y, string z)
        {
            FilterDefinition<dynamic> searchFilter = FilterDefinition<dynamic>.Empty;

            var startDate = DateTime.Parse(y, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var endDate = DateTime.Parse(z, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            searchFilter &= Builders<dynamic>.Filter.Gte(x, startDate);
            searchFilter &= Builders<dynamic>.Filter.Lt(x, endDate);

            return searchFilter;
        }

        private FilterDefinition<dynamic> GetFilterDefinitionWithBsonDocument(string y)
        {
            FilterDefinition<dynamic> searchFilter = FilterDefinition<dynamic>.Empty;

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
                        y
                    }
                )
            );

            searchFilter &= filterDoc;

            return searchFilter;
        }

        private void PrintOutput(List<dynamic> result)
        {
            foreach (var movie in result)
            {
                Console.WriteLine("Title: {0}, Last Updated: {1}"
                    , movie["title"].ToString()
                    , Convert.ToDateTime(movie["lastupdated"]).ToString("yyyy-MM-dd HH:mm:ss"));
            }

            Console.WriteLine("Count: {0}", result.Count);
        }
    }
}
