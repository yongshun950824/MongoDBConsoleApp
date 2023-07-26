using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/75240005/c-sharp-mongodb-select-rows-by-their-sequence-number-in-one-query/75242146#75242146">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_062 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<ContentEntity> _contentEntities = _db.GetCollection<ContentEntity>("content_entity");

            ObjectId contentId = new ObjectId("63d1f7c7c499d138a6676c71");
            CancellationToken cancellationToken = new CancellationToken();

            var projection = new BsonDocument
            {
                {
                    "Lines", new BsonDocument
                    {
                        {
                            "$filter", new BsonDocument
                            {
                                { "input", "$Lines" },
                                {
                                    "cond", new BsonDocument
                                    {
                                        {
                                            "$in", new BsonArray
                                            {
                                                new BsonDocument("$indexOfArray", new BsonArray
                                                {
                                                    "$Lines",
                                                    "$$this"
                                                }),
                                                new BsonArray { 0, 1, 3 }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var entity = await _contentEntities
                .Find(e => e.Id == contentId)
                .Project<ContentEntity>(projection)
                .FirstOrDefaultAsync(cancellationToken);

            Helpers.PrintFormattedJson(entity);
        }

        class ContentEntity
        {
            [BsonId]
            public ObjectId Id { get; set; }

            [BsonElement("Lines")]
            public List<string> Lines2 { get; set; }
        }
    }
}
