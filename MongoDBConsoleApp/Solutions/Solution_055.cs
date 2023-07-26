using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
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
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<ClassStudents> collection = _db.GetCollection<ClassStudents>("classStudents");

            FilterDefinitionBuilder<ClassStudents> builder = Builders<ClassStudents>.Filter;

            var filter = builder.Eq(x => x.Class, "Math");

            #region Solution 1: ElemMatch with LINQ Expression
            //filter &= builder.ElemMatch(x => x.Students, y => y.FullName == "Dan");
            #endregion

            #region Solution 2: ElemMatch with FilterDefinition
            //filter &= builder.ElemMatch(x => x.Students,
            //    Builders<Student>.Filter.Eq(y => y.FullName, "Dan"));
            #endregion

            #region Solution 3: With regex match
            filter &= builder.ElemMatch(x => x.Students,
                Builders<Student>.Filter.Regex(y => y.FullName, "Dan"));
            #endregion

            var document = await collection.Find(filter)
                .FirstOrDefaultAsync();

            Helpers.PrintFormattedJson(document);
        }

        class ClassStudents
        {
            public ObjectId Id { get; set; }

            [BsonElement("id")]
            public string ClassId { get; set; }

            [BsonElement("Class")]
            public string Class { get; set; }

            public List<Student> Students { get; set; }
        }

        class Student
        {
            public string FullName { get; set; }
        }
    }
}
