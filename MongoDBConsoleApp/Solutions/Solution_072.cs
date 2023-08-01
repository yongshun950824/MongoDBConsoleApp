using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/76803275/mongodb-c-sharp-property-serializing-string-and-int-for-query/76804531#76804531">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_072 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            int intStatus = 1;
            string stringStatus = "One";

            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<A> _collection = _database.GetCollection<A>("A");

            var filterCondition = Builders<A>.Filter.Empty;

            #region Solution 1
            //var f = Builders<A>.Filter.Or(
            //    new BsonDocument("Status.Code", intStatus),
            //    Builders<A>.Filter.Eq("Status.Code", stringStatus)
            //);
            #endregion

            #region Solution 2
            var f = Builders<A>.Filter.Or(
                Builders<A>.Filter.Eq("Status.Code", BsonValue.Create(intStatus)),
                Builders<A>.Filter.Eq("Status.Code", stringStatus)
            );
            #endregion

            filterCondition &= f;
            var result = (await _collection.FindAsync(filterCondition))
                .ToList();

            Helpers.PrintFormattedJson(result);
        }

        class A
        {
            public ObjectId Id { get; set; }
            public Status<MyEnum> Status { get; set; }
        }

        class Status<T>
        {
            [BsonRepresentation(BsonType.String)] //<- this was added some time later!
            public T Code { get; set; }
            public DateTime Timestamp { get; set; }
            public int UserId { get; set; }
        }

        enum MyEnum
        {
            One = 1,
            Two
        }
    }
}
