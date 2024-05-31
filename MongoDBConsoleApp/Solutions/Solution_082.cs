using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/78417280/how-to-perform-operations-on-grandchild-using-builders/78417532#78417532">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_082 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            string parentKey = "1";
            string childKey = "2";
            string grandChildKey = "3";

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Parent> _collection = _db.GetCollection<Parent>("parent");


            var filter = Builders<Parent>.Filter.And(
                Builders<Parent>.Filter.Eq(p => p.ParentKey, parentKey),
                Builders<Parent>.Filter.ElemMatch(p => p.Children,
                    Builders<Child>.Filter.And(
                        Builders<Child>.Filter.Eq(c => c.ChildKey, childKey),
                            Builders<Child>.Filter.ElemMatch(c => c.GrandChildren,
                                Builders<GrandChild>.Filter.Eq(c => c.GrandChildKey, grandChildKey)
                            )
                    )
                )
            );

            var pushDefinition = Builders<Parent>.Update.Push("children.$[c].grandChildren.$[gc].greatGrandChildren", new GreatGrandChild
            {
                GreatGrandChildKey = "6",
                SomeValue = "SomeValue 6"
            });

            var result = await _collection.UpdateOneAsync(filter, pushDefinition,
                new UpdateOptions
                {
                    ArrayFilters = new[]
                    {
                        new BsonDocumentArrayFilterDefinition<BsonDocument>(
                            new BsonDocument("c.childKey", childKey)
                        ),
                        new BsonDocumentArrayFilterDefinition<BsonDocument>(
                            new BsonDocument("gc.grandChildKey", grandChildKey)
                        ),
                    }
                });

            Helpers.PrintFormattedJson(result);
        }

        class Parent
        {
            public ObjectId Id { get; set; }
            public string ParentKey { get; set; }
            public IEnumerable<Child> Children { get; set; }
        }

        class Child
        {
            public string ChildKey { get; set; }
            public IEnumerable<GrandChild> GrandChildren { get; set; }
        }

        class GrandChild
        {
            public string GrandChildKey { get; set; }
            public IEnumerable<GreatGrandChild> GreatGrandChildren { get; set; }
        }

        class GreatGrandChild
        {
            public string GreatGrandChildKey { get; set; }
            public string SomeValue { get; set; }
        }
    }
}
