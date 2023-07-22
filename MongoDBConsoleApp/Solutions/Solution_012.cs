using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70753460/mongodb-net-driver-update-cannot-use-the-part-to-traverse-the-element/70754899#70754899">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_012 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var collection = _database.GetCollection<VisitTask>("visitTask");

            var orderFilter = Builders<VisitTask>.Filter.Empty;

            #region Solution 1
            UpdateDefinition<VisitTask> update = GetUpdateDefinitionWithPipeline();
            #endregion

            #region Solution 2
            //UpdateDefinition<VisitTask> update = GetUpdateDefinitionWithBsonDocument();
            #endregion

            collection.UpdateMany(orderFilter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        private UpdateDefinition<VisitTask> GetUpdateDefinitionWithPipeline()
        {
            UpdateDefinition<VisitTask> update = Builders<VisitTask>.Update.Pipeline(
                new PipelineStagePipelineDefinition<VisitTask, VisitTask>(
                    new PipelineStageDefinition<VisitTask, VisitTask>[]
                    {
                        new BsonDocument("$set",
                            new BsonDocument("Orders",
                                new BsonDocument("$map",
                                new BsonDocument
                                {
                                    { "input", "$Orders" },
                                    { "in",
                                        new BsonDocument("$mergeObjects",
                                            new BsonArray
                                            {
                                                new BsonDocument("Number-test", "$$this.Number"),
                                                "$$this"
                                            })
                                    }
                                }))),
                        new BsonDocument("$unset", "Orders.Number")
                    }));

            return update;
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <returns></returns>
        private UpdateDefinition<VisitTask> GetUpdateDefinitionWithBsonDocument()
        {
            UpdateDefinition<VisitTask> update = Builders<VisitTask>.Update.Pipeline(
                new BsonDocument[]
                {
                    new BsonDocument("$set",
                        new BsonDocument("Orders",
                            new BsonDocument("$map",
                            new BsonDocument
                            {
                                { "input", "$Orders" },
                                { "in",
                                    new BsonDocument("$mergeObjects",
                                        new BsonArray
                                        {
                                            new BsonDocument("Number-test", "$$this.Number"),
                                            "$$this"
                                        })
                                }
                            }))),
                    new BsonDocument("$unset", "Orders.CustomFields.Пара паков")
                }
            );

            return update;
        }

        class Visit
        {
            public ObjectId Id { get; set; }
            public CustomField CustomFields { get; set; }
        }

        class CustomField
        {
            [BsonElement("Сheckpoint Comment")]
            public BsonProp Checkpoint { get; set; }
            [BsonElement("Time of arrival at the checkpoint")]
            public BsonProp Time { get; set; }
        }

        class BsonProp
        {
            public ObjectId FieldId { get; set; }
            public string Type { get; set; }
            public string ValueBson { get; set; }
        }

        class VisitTask
        {
            public ObjectId Id { get; set; }
            public DateTime LastUpdatedDate { get; set; }
            public List<Order> Orders { get; set; }
        }

        class Order
        {
            public CustomFields CustomFields { get; set; }
        }

        class CustomFields
        {
            [BsonElement("Пара паков")]
            public BsonProp Napa { get; set; }
        }
    }
}
