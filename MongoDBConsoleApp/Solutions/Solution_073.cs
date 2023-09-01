using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/76964075/mongodb-c-sharp-driver-convert-time-buckets-to-a-dictionary-with-key-as-bucket/76966121#76966121">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_073 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Transaction> _collection = _db.GetCollection<Transaction>("transaction");

            FilterDefinition<Transaction> licenseFilter = Builders<Transaction>.Filter
                .Eq(X => X.licenseId, Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"));

            PipelineStageDefinition<Transaction, Transaction> firstStage
                = PipelineStageDefinitionBuilder.Match(licenseFilter);

            PipelineStageDefinition<Transaction, BsonDocument> secondStage
                = PipelineStageDefinitionBuilder.Group<Transaction, BsonDocument>(new BsonDocument
                {
                    { "_id", "$transactionDay" },
                    { "licenseId", new BsonDocument
                        {
                            { "$first", "$licenseId" }
                        }
                    },
                    { "sizeTotal", new BsonDocument
                        {
                            { "$sum", "$size" }
                        }
                    },
                    { "countTotal", new BsonDocument
                        {
                            { "$sum", "$count" }
                        }
                    }
                });

            PipelineStageDefinition<BsonDocument, BsonDocument> thirdStage
                = PipelineStageDefinitionBuilder.Group<BsonDocument, BsonDocument>(new BsonDocument
                {
                    { "_id", "$licenseId" },
                    { "transactionDays", new BsonDocument
                        {
                            { "$push", new BsonDocument
                                {
                                    { "k", "$_id" },
                                    { "v", new BsonDocument
                                        {
                                            { "countTotal", "$countTotal" },
                                            { "sizeTotal", "$sizeTotal" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });

            PipelineStageDefinition<BsonDocument, UsageReport> forthStage
                = PipelineStageDefinitionBuilder.Project<BsonDocument, UsageReport>(new BsonDocument
                {
                    { "_id", 0 },
                    { "licenseId", "$_id" },
                    { "usage", new BsonDocument("$arrayToObject", "$transactionDays") }
                });

            var usageReport = _collection.Aggregate()
                .AppendStage(firstStage)
                .AppendStage(secondStage)
                .AppendStage(thirdStage)
                .AppendStage(forthStage)
                .ToList();

            Helpers.PrintFormattedJson(usageReport);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        class Transaction
        {
            public ObjectId Id { get; set; }
            [BsonRepresentation(BsonType.String)]
            public Guid licenseId { get; set; }
            public string transactionDay { get; set; }
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public int count { get; set; }
            public int size { get; set; }
        }

        [BsonNoId]
        class UsageReport
        {
            public Guid licenseId { get; set; }
            public Dictionary<string, UsageDate> usage { get; set; }
        }

        class UsageDate
        {
            public int countTotal { get; set; }
            public int sizeTotal { get; set; }
        }
    }
}
