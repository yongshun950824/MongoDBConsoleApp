using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/71693040/how-can-i-flatten-this-array-of-subdocuments/71693509#71693509">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_024 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            var collection = _db.GetCollection<Course>("course");

            var topics = await collection
                .Aggregate()
                .Unwind<Course>(c => c.Topics)
                //.Project("{ topics: 1, _id: 0 }")
                .Project(new BsonDocument { { "topics", 1 }, { "_id", 0 } })
                .ReplaceRoot<Topic>(newRoot: "$topics")
                .ToListAsync();

            Helpers.PrintFormattedJson(topics);
        }

        class Course
        {
            [BsonId]
            public ObjectId Id { get; set; }
            [BsonRepresentation(BsonType.ObjectId)]
            public string CourseId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public List<Topic> Topics { get; set; }
        }

        class Topic
        {
            [BsonRepresentation(BsonType.ObjectId)]
            public string TopicId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            public List<LessonSummary> Lessons { get; set; }

        }

        class LessonSummary
        {
            public string LessonId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }
    }
}
