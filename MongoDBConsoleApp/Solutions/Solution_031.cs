using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    internal class Solution_031 : ISolution
    {
        /// <summary>
        /// <a href="https://stackoverflow.com/questions/73299041/how-get-all-keys-in-all-level-in-a-mongo-collection-c-linq-functions">
        /// Question.
        /// </a>
        /// </summary>
        /// <param name="_client"></param>
        public void Run(IMongoClient _client)
        {
            var database = _client.GetDatabase("demo");
            var collection = database.GetCollection<BsonDocument>("Solution_031");

            var query = collection.AsQueryable<BsonDocument>();
            BsonDocument bson = query.FirstOrDefault();
            List<string> collectionKeys = GetRootDocumentAllKeys(bson);

            Console.WriteLine(String.Join(",", collectionKeys.Distinct()));
        }

        private List<string> GetArrayKeys(BsonValue value)
        {
            List<string> keys = new List<string>();

            if (value == null)
                return keys;

            if (value.GetType() != typeof(BsonArray))
                return keys;

            foreach (var item in (BsonArray)value)
            {
                keys.AddRange(GetArrayKeys(item));
                keys.AddRange(GetDocumentKeys(item));
            }

            return keys;
        }

        private List<string> GetRootDocumentAllKeys(BsonDocument bson)
        {
            List<string> collectionKeys = new List<string>();
            collectionKeys.AddRange(GetDocumentKeys(bson));

            return collectionKeys.Distinct()
                .ToList();
        }

        private List<string> GetDocumentKeys(BsonValue value)
        {
            List<string> keys = new List<string>();

            if (value == null)
                return keys;

            if (value.GetType() != typeof(BsonDocument))
                return keys;

            foreach (var kvp in (BsonDocument)value)
            {
                keys.Add(kvp.Name);

                keys.AddRange(GetArrayKeys(kvp.Value));
                keys.AddRange(GetDocumentKeys(kvp.Value));
            }

            return keys;
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }
    }
}
