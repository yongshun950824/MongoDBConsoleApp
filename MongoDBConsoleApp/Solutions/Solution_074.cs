using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/75315776/how-to-update-different-fields-based-on-conditions-in-single-call/75317967#75317967">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_074 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            BsonClassMap.RegisterClassMap<Job>(
                map =>
                {
                    map.AutoMap();
                    map.MapProperty(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
                });

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Job> _jobCollection = _db.GetCollection<Job>("job");

            List<Guid> A = new List<Guid>
            {
                Guid.Parse("bcb86939-7fb4-4d5e-b205-eef00d46c6a2")
            };

            List<Guid> B = new List<Guid>
            {
                Guid.Parse("2b6c7ae0-7607-4ccf-9d9c-0f217244b821"),
                Guid.Parse("eda42c07-c74b-4e22-8378-7de5f5c6f8a6"),
            };

            FilterDefinition<Job> filter = Builders<Job>.Filter.In(x => x.Id, A.Concat(B));

            UpdateDefinition<Job> update = Builders<Job>.Update.Pipeline(new BsonDocument[]
            {
                new BsonDocument("$set",
                    new BsonDocument
                    {
                        {
                            "IsPending",
                            new BsonDocument("$cond",
                                new BsonDocument
                                {
                                   {
                                        "if",
                                        new BsonDocument(
                                            "$in",
                                            new BsonArray
                                            {
                                                "$_id",
                                                BsonArray.Create(A.Select(x => x.ToString()).ToList())
                                            }
                                        )
                                    },
                                    { "then", true },
                                    { "else", "$IsPending" }
                                }
                            )
                        },
                        {
                            "IsComplete",
                            new BsonDocument("$cond",
                                new BsonDocument
                                {
                                   {
                                        "if",
                                        new BsonDocument(
                                            "$in",
                                            new BsonArray
                                            {
                                                "$_id",
                                                BsonArray.Create(B.Select(x => x.ToString()).ToList())
                                            }
                                        )
                                    },
                                    { "then", true },
                                    { "else", "$IsComplete" }
                                }
                            )
                        }
                    }
                )
            });

            UpdateResult updateResult = await _jobCollection.UpdateManyAsync(filter, update);
            Helpers.PrintFormattedJson(updateResult);
        }
    }

    internal class Job
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }

        public bool IsPending { get; set; }

        public bool IsComplete { get; set; }
    }
}
