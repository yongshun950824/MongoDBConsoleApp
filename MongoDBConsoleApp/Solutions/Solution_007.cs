using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70298251/how-to-filter-and-get-last-entry-based-on-date-using-c-sharp-and-mongo/70299082#70299082">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_007 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var collection = _database.GetCollection<BsonDocument>("student");

            string[] students = { "stu-abc", "stu-1234" };
            string dept = "geog";

            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match",
                    new BsonDocument
                    {
                        { "studentid",
                            new BsonDocument("$in",
                                BsonArray.Create(students))
                        },
                        { "dept", dept }
                    }
                ),
                new BsonDocument("$sort",
                new BsonDocument("Carddetails.LastSwipeTimestamp", -1)),
                new BsonDocument("$group",
                new BsonDocument
                    {
                        { "_id",
                            new BsonDocument
                            {
                                { "studentid", "$studentid" },
                                { "dept", "$dept" }
                            }
                        },
                        { "Carddetails",
                            new BsonDocument("$first", "$Carddetails")
                        }
                    }
                ),
                new BsonDocument("$project",
                    new BsonDocument
                    {
                        { "_id", 0 },
                        { "studentid", "$_id.studentid" },
                        { "dept", "$_id.dept" },
                        { "Carddetails", "$Carddetails" }
                    }
                )
            };

            var result = collection.Aggregate<BsonDocument>(pipeline)
                .ToList();

            Helpers.PrintFormattedJson(result);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }
    }
}
