using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    class Solution_042 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>("User");

            //Guid id = Guid.NewGuid();
            ObjectId id = ObjectId.Parse("6317f517ce0c4813ece18b66");

            #region Init Data
            //InitData(collection, id);
            #endregion

            //var filter = Builders<BsonDocument>.Filter.Eq("Id", id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var addressDocument = new BsonDocument
            {
                { "City", "New City Value 2" },
                { "State", "New State Value 2" }
            };

            var update = Builders<BsonDocument>.Update.Pipeline(new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>
            (
                new PipelineStageDefinition<BsonDocument, BsonDocument>[]
                {
                    new BsonDocument("$set",
                        new BsonDocument("Address",
                            new BsonDocument("$cond",
                                new BsonDocument
                                {
                                    {
                                        "if",
                                        new BsonDocument("$eq",
                                            BsonArray.Create(new object[] { "$Address", null }))
                                    },
                                    {
                                        "then",
                                        addressDocument
                                    },
                                    {
                                        "else",
                                        new BsonDocument("$mergeObjects",
                                            BsonArray.Create(new object[] { "$Address", addressDocument }))
                                    }
                                }
                            )
                        )
                    )
                }
            ));
            var bsonDocument = await collection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After
            });

            Console.WriteLine(bsonDocument.ToJson(new MongoDB.Bson.IO.JsonWriterSettings
            {
                Indent = true
            }));
        }

        private async void InitData(IMongoCollection<BsonDocument> collection, Guid id)
        {
            var user = new BsonDocument
            {
                { "Id", BsonValue.Create(id) },
                { "Name", "Adrian" }
            };

            await collection.InsertOneAsync(user);
        }
    }

    public class User
    {
        public Guid? Id { get; set; }
        public String? Name { get; set; }
        public Address? Address { get; set; }
    }

    public class Address
    {
        public String? Street { get; set; }
        public String? City { get; set; }
        public String? State { get; set; }
    }
}
