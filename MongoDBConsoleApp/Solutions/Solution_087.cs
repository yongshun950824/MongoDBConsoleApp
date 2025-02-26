using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/79433036/mongodb-count-number-of-object-in-array/79433069#79433069">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_087 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Class> _col = _db.GetCollection<Class>("Class");

            ObjectId id = ObjectId.Parse("630811c0867dcfbaeb3cf61a");

            Helpers.PrintFormattedJson(GetStudentCountFromClassWithFluent(_col, id));
            Helpers.PrintFormattedJson(GetStudentCountFromClassWithBsonQuery(_col, id));
        }

        public class Class
        {
            public ObjectId Id { get; set; }
            public string Name { get; set; }
            public Student[] Students { get; set; }
        }

        public class Student
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int StudentId { get; set; }
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <param name="_col"></param>
        /// <returns></returns>
        private int GetStudentCountFromClassWithFluent(IMongoCollection<Class> collection, ObjectId id)
        {
            return collection.Aggregate()
                .Match(x => x.Id == id)
                .Project(x => new
                {
                    Count = x.Students.Count()
                })
                .FirstOrDefault()?.Count ?? 0;
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <param name="_col"></param>
        /// <returns></returns>
        private int GetStudentCountFromClassWithBsonQuery(IMongoCollection<Class> collection, ObjectId id)
        {
            PipelineDefinition<Class, BsonDocument> pipline = new BsonDocument[]
            {
                new BsonDocument("$match",
                    new BsonDocument("_id",
                        id)),
                new BsonDocument("$project",
                    new BsonDocument("count",
                        new BsonDocument("$size", "$Students")))
            };

            return collection.Aggregate(pipline)
                .FirstOrDefault()
                ?["count"].AsInt32 ?? 0;
        }
    }
}
