using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/72968801/count-unread-messages-in-mongodb/72974256#72974256">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_028 : ISolution
    {
        public async Task RunAsync(IMongoClient _client)
        {
            int userId = 1;
            Console.WriteLine($"Count: {await UserUnreadMessagesCount(_client, userId)}");
        }

        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private async Task<int> UserUnreadMessagesCount(IMongoClient _client, int userId)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<ChatGroup> _collection = _database.GetCollection<ChatGroup>("ChatGroup");

            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$set",
                    new BsonDocument("Members",
                        new BsonDocument("$filter",
                            new BsonDocument
                            {
                                { "input", "$Members" },
                                { "cond", new BsonDocument
                                    (
                                        "$and", new BsonArray
                                        {
                                            new BsonDocument("$eq",
                                                new BsonArray { "$$this.UserId", userId }),
                                            new BsonDocument("$gt",
                                                new BsonArray { "$LastMessageDate", "$$this.LastReadDate" })
                                        }
                                    )
                                }
                            }
                        )
                    )
                ),
                new BsonDocument("$match",
                    new BsonDocument("Members",
                        new BsonDocument("$ne", new BsonArray())))

            };

            return (await _collection.AggregateAsync<BsonDocument>(pipeline))
                .ToList()
                .Count;
        }

        class ChatGroup
        {
            [BsonId]
            internal int Id { get; set; }

            internal DateTime LastMessageDate { get; set; }

            internal ChatGroupMember[] Members { get; set; }
        }

        class ChatGroupMember
        {
            internal int UserId { get; set; }
            internal DateTime LastReadDate { get; set; }
        }
    }
}
