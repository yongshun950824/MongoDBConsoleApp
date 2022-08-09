using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73267736/c-sharp-searching-mongodb-string-that-starts-with-xyz/73267990#73267990">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_030 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("sample_mflix");
            var _collection = _database.GetCollection<BsonDocument>("movies");

            string searchTerm = "The";
            var results = _collection.Find(Filter1(searchTerm))
                .ToList();

            PrintOutput(results);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        private FilterDefinition<BsonDocument> Filter1(string searchTerm)
        {
            return Builders<BsonDocument>.Filter.Regex("title"
                , new Regex("^" + searchTerm));
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        private FilterDefinition<BsonDocument> Filter2(string searchTerm)
        {
            return Builders<BsonDocument>.Filter.Regex("title"
                , new BsonRegularExpression("^" + searchTerm));
        }

        private void PrintOutput(dynamic results)
        {
            foreach (var movie in results)
            {
                Console.WriteLine("{0}", movie["title"].ToString());
            }
        }
    }
}
