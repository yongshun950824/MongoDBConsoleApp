using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/69601591/mongodb-net-driver-startswith-contains-with-loosely-typed-data/69601745#69601745">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_004 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("sample_mflix");
            var _collection = _database.GetCollection<BsonDocument>("movies");
            var query = _collection.AsQueryable();

            var result = query.Where(Filter3())
                .Take(10);

            PrintOutput(result);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        /// <summary>
        /// Solution 1: Equal
        /// </summary>
        /// <returns></returns>
        private Expression<Func<BsonDocument, bool>> Filter1()
        {
            return x => x["title"] == "The";
        }

        /// <summary>
        /// Solution 2: Contain
        /// </summary>
        /// <returns></returns>
        private Expression<Func<BsonDocument, bool>> Filter2()
        {
            return x => ((string)x["title"]).Contains("The");
        }

        /// <summary>
        /// Solution 3: Start with
        /// </summary>
        /// <returns></returns>
        private Expression<Func<BsonDocument, bool>> Filter3()
        {
            var filter = Builders<BsonDocument>
                .Filter
                .Regex("title", "^" + "The" + ".*");
                //.Regex("title", "the");

            return x => filter.Inject();
        }

        private void PrintOutput(dynamic result)
        {
            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }
    }
}
