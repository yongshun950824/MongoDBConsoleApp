using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MongoDBConsoleApp.Program;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73488700/mongodb-net-driver-aggregate-group-and-count/73499788#73499788">
    /// Question
    /// </a>
    /// </summary>
    internal class Solution_036 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Box> _collection = _db.GetCollection<Box>("box");

            PipelineStageDefinition<Box, BsonDocument> firstStage
                = PipelineStageDefinitionBuilder.Group<Box, BsonDocument>(new BsonDocument
                {
                    { "_id", new BsonDocument
                        {
                            { "warehouseId", "$warehouseId" },
                            { "content", "$content" }
                        }
                    },
                    { "count", new BsonDocument
                        {
                            { "$sum", 1 }
                        }
                    }
                });

            PipelineStageDefinition<BsonDocument, BsonDocument> secondStage
                = PipelineStageDefinitionBuilder.Group<BsonDocument, BsonDocument>(new BsonDocument
                {
                    { "_id", "$_id.warehouseId" },
                    { "boxes", new BsonDocument
                        {
                            { "$push", new BsonDocument
                                {
                                    { "k", "$_id.content" },
                                    { "v", "$count" }
                                }
                            }
                        }
                    }
                });

            PipelineStageDefinition<BsonDocument, BsonDocument> thirdStage
                = PipelineStageDefinitionBuilder.ReplaceRoot<BsonDocument, BsonDocument>(new BsonDocument
                {
                    { "$mergeObjects", new BsonArray
                        {
                            new BsonDocument("Warehouse", "$_id"),
                            new BsonDocument("$arrayToObject", "$boxes")
                        }
                    }
                });

            PipelineStageDefinition<BsonDocument, BsonDocument> forthStage
                = PipelineStageDefinitionBuilder.Sort(Builders<BsonDocument>.Sort.Ascending("Warehouse"));

            var result = _collection.Aggregate()
                .AppendStage(firstStage)
                .AppendStage(secondStage)
                .AppendStage(thirdStage)
                .AppendStage(forthStage)
                .ToList();

            PrintOutput(result);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private void PrintOutput(List<BsonDocument> result)
        {
            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true,
            }));
        }

        class Box
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public string Content { get; set; }
            public string WarehouseId { get; set; }
        }
    }
}
