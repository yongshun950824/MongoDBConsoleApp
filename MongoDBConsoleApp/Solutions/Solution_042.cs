using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73628953/mongodb-cannot-create-field-childproperty-in-element-with-parentproperty-is/73629356#73629356">
    /// Question
    /// </a>
    /// </summary>
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

            ObjectId id = ObjectId.Parse("6317f517ce0c4813ece18b66");

            //var filter = Builders<BsonDocument>.Filter.Eq("Id", id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var addressDocument = new BsonDocument
            {
                { "City", "New City Value 2" },
                { "State", "New State Value 2" }
            };
            string name = "";

            var update = Builders<BsonDocument>.Update.Pipeline(new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>
            (
                new PipelineStageDefinition<BsonDocument, BsonDocument>[]
                {
                    new BsonDocument("$set",
                        new BsonDocument
                        {
                            { "Name", name },
                            { "Address", new BsonDocument("$cond",
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
                            },
                        }
                    )
                }
            ));

            var bsonDocument = await collection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After
            });

            PrintOutput(bsonDocument);
        }

        private void PrintOutput(BsonDocument bsonDocument)
        {
            Console.WriteLine(bsonDocument.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
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
