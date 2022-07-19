using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70795342/convert-lambda-expressions-to-json-objects-using-mongodb-driver">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_015 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var _collection = _database.GetCollection<Person>("person");

            FilterDefinition<Person> filter = Builders<Person>.Filter.Empty;
            //filter &= Builders<Person>.Filter.Where(x => ((string)x["Name"]).Contains("Rob")
            //    && (DateTime)x["DateCreated"] < DateTime.UtcNow);

            var jsonQuery = _collection.ExpressionToJson<Person>(x => x.Name.Contains("Tono")
                && x.DateCreated < DateTime.UtcNow);
            //var jsonQuery = _collection.ExpressionToJson<Person>(x => ((string)x["Name"]).Contains("Rob")
            //    && (DateTime)x["DateCreated"] < DateTime.UtcNow);

            Console.WriteLine(jsonQuery);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        class Person
        {
            [BsonId]
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime DateCreated { get; set; }
        }
    }
}

namespace MongoDBConsoleApp
{
    public static class IMongoCollectionExtensions
    {
        public static string ExpressionToJson<T>(this IMongoCollection<T> collection, 
            Expression<Func<T, bool>> filter)
        {
            var query = collection.Find(filter);

            return query.Filter.Render(
                collection.DocumentSerializer,
                collection.Settings.SerializerRegistry
                ).ToJson();
        }
    }
}
