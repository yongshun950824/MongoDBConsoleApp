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
    /// <a href="https://stackoverflow.com/questions/70660236/retrieving-list-of-documents-from-collection-by-id-in-nested-list/70660552#70660552">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_008 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var _certificats = _database.GetCollection<Example>("random");

            int porduitId = 17;

            var cursor = await _certificats.FindAsync(GetFilterWithBsonDocument(porduitId));
            var docs = cursor.ToList();

            PrintOutput(docs);
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        private FilterDefinition<Example> GetFilterWithFilterDefinition(int porduitId)
        {
            return Builders<Example>.Filter.ElemMatch(
                x => x.Family,
                y => y.Countries.Any(z => z.uid == porduitId));
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <param name="porduitId"></param>
        /// <returns></returns>
        private BsonDocument GetFilterWithBsonDocument(int porduitId)
        {
            return new BsonDocument("Family.Countries.uid", porduitId);
        }

        private void PrintOutput(List<Example> docs)
        {
            foreach (var doc in docs)
                Console.WriteLine(doc.ToJson());
        }

        class Example
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public string Source { get; set; }
            public List<FamilyCountry> Family { get; set; }
        }

        class FamilyCountry
        {
            public List<Country> Countries { get; set; }
        }

        class Country
        {
            public int uid { get; set; }
            public string name { get; set; }
        }
    }
}
