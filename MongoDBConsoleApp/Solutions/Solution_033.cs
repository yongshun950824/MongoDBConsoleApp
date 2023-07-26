using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73352502/mongodb-search-a-field-of-type-bsondocument-by-their-values/73356181#73356181">
    /// Question
    /// </a>
    /// </summary>
    internal class Solution_033 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<UserInfo> _collection = _db.GetCollection<UserInfo>("UserInfo");

            BsonDocument greaterThanCriteria =
                new BsonDocument() {
                    { "$gt", new BsonArray() {
                        new BsonDocument() {
                            { "$size", new BsonDocument() {
                                { "$filter", new BsonDocument() {
                                    { "input", new BsonDocument()
                                        {
                                            { "$objectToArray", "$UserThemes"  }
                                        }
                                    },
                                    { "cond", new BsonDocument()
                                        {
                                            { "$gt", new BsonArray() {
                                                "$$this.v", 100
                                                }
                                            }
                                        }
                                    }
                                  }
                                }
                              }
                            }
                          },
                        0
                      }
                    }
                };

            var docs = await _collection.Aggregate()
                .Match(
                    new BsonDocument() {
                       { "$expr", new BsonDocument() {
                            { "$and", new BsonArray() {
                                  greaterThanCriteria
                               }
                            }
                         }
                       }
                    })
              .As<UserInfo>()
              .ToListAsync();

            Helpers.PrintFormattedJson(docs);
        }

        class UserInfo
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public BsonDocument UserThemes { get; set; } = new BsonDocument();
        }
    }
}
