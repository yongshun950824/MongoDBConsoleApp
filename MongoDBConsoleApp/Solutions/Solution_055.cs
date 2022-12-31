using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/74883970/newbie-to-cosmodb-how-to-query-collection-with-multiple-values/74884127#74884127">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_055 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            this.RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<ClassStudents> collection = _db.GetCollection<ClassStudents>("classStudents");

            FilterDefinitionBuilder<ClassStudents> builder = Builders<ClassStudents>.Filter;

            var filter = builder.Eq(x => x.Class, "Math");

            // Solution 1: ElemMatch with LINQ Expression
            //filter &= builder.ElemMatch(x => x.Students, y => y.FullName == "Dan");

            // Solution 2: ElemMatch with FilterDefinition
            //filter &= builder.ElemMatch(x => x.Students,
            //    Builders<Student>.Filter.Eq(y => y.FullName, "Dan"));

            // Solution: With regex match
            filter &= builder.ElemMatch(x => x.Students, 
                Builders<Student>.Filter.Regex(y => y.FullName, "Dan"));

            var document = await collection.Find(filter)
                .FirstOrDefaultAsync();

            Console.WriteLine(JsonConvert.SerializeObject(document, Formatting.Indented));
        }

        internal class ClassStudents
        {
            public ObjectId Id { get; set; }

            [BsonElement("id")]
            public string ClassId { get; set; }

            [BsonElement("Class")]
            public string Class { get; set; }

            public List<Student> Students { get; set; }
        }

        internal class Student
        {
            public string FullName { get; set; }
        }
    }
}
