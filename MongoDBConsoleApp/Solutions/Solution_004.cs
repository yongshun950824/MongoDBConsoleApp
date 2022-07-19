using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDBConsoleApp
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

            var results = query.Where(Filter3())
                .Take(10);

            PrintOutput(results);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <returns></returns>
        private Expression<Func<BsonDocument, bool>> Filter1()
        {
            return x => x["first_name"] == "john";
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <returns></returns>
        private Expression<Func<BsonDocument, bool>> Filter2()
        {
            return x => ((string)x["first_name"]).Contains("john");
        }

        /// <summary>
        /// Solution 3
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

        private void PrintOutput(dynamic results)
        {
            foreach (var movie in results)
            {
                Console.WriteLine("{0}", movie["title"].ToString());
            }
        }
    }
}
