using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using static MongoDBConsoleApp.Program;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73437201/mongodb-return-filtered-array-elements-out-of-one-document-in-c-sharp/73439241#73439241">
    /// Question
    /// </a>
    /// </summary>
    internal class Solution_034 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<CustomEntity> _collection = _db.GetCollection<CustomEntity>("CustomEntity");

            FilterDefinition<CustomEntity> filter = Builders<CustomEntity>.Filter.ElemMatch(x => x.Logs,
                Builders<LogEntity>.Filter.Eq(y => y.DataType, "System.string"));

            ProjectionDefinition<CustomEntity> projection =
                new BsonDocument
                {
                    { "logs", new BsonDocument(
                            "$filter", new BsonDocument
                            {
                                { "input", "$logs" },
                                { "cond", new BsonDocument(
                                    "$eq", 
                                    BsonArray.Create(new string[] { "$$this.dataType", "System.string" })
                                    ) 
                                }
                            }
                        )
                    }
                };

            var result = _collection.Find(filter)
                .Project<CustomEntity>(projection)
                .ToList();

            PrintOutput(result);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private void PrintOutput(dynamic result)
        {
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }

    public class CustomEntity
    {
        [BsonId]
        public string Id { get; set; }
        public LogEntity[] Logs { get; set; }
    }

    public class LogEntity
    {
        public string Data { get; set; }
        public string DataType { get; set; }
    }
}
