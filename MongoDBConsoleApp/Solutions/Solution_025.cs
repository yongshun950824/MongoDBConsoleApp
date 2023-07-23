using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/72170723/how-to-aggregate-with-project-priority-in-mongodb-query-in-c/72171247#72171247">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_025 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            var collection = _db.GetCollection<Item>("item");

            FilterDefinition<Item> filterDefinition = Builders<Item>.Filter.Empty;
            ProjectionDefinition<Item> projection = GetProjectionDefinitionWithConcreteType();
            SortDefinition<Item> sort = Builders<Item>.Sort.Descending("priority");

            var result = await collection.Aggregate()
                .Match(filterDefinition)
                //.Facet(countFacet, dataFacet)
                .Project<Item>(projection)
                .Sort(sort)
                .ToListAsync();

            PrintOutput(result);
        }

        private ProjectionDefinition<BsonDocument> GetProjectionDefinitionWithBsonDocument()
        {
            ProjectionDefinition<BsonDocument> projection = new BsonDocument
            {
                { "category", 1 },
                { "title", 1 },
                { "priority",
                    new BsonDocument(
                        "$eq", new BsonArray
                        {
                            "$_id",
                            2
                        }
                   )
                }
            };

            return projection;
        }

        private ProjectionDefinition<Item> GetProjectionDefinitionWithConcreteType()
        {
            ProjectionDefinition<Item> projection = new BsonDocument
            {
                { "category", 1 },
                { "title", 1 },
                { "priority",
                    new BsonDocument(
                        "$eq", new BsonArray
                        {
                            "$_id",
                            2
                        }
                   )
                }
            };

            return projection;
        }

        private void PrintOutput(dynamic result)
        {
            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }

        class Item
        {
            [BsonId]
            public int Id { get; set; }
            [BsonElement("category")]
            public string Category { get; set; }
            [BsonElement("title")]
            public string Title { get; set; }
            [BsonElement("priority")]
            public bool Priority { get; set; }
        }
    }
}
