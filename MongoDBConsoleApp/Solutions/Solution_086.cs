using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/78921190/mongodb-c-sharp-how-to-do-sorting-and-pagination-with-an-array-of-objects-on-a/78921324#78921324">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_086 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<People> _col = _db.GetCollection<People>("people");

            #region Solution 1
            Helpers.PrintFormattedJson(await SortPeopleWithBsonQuery(_col));
            #endregion

            #region Solution 2
            Helpers.PrintFormattedJson(await SortPeopleWithFluentAggregate(_col));
            #endregion
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <param name="_col"></param>
        /// <returns></returns>
        private async Task<List<People>> SortPeopleWithBsonQuery(IMongoCollection<People> _col)
        {

            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$set",
                    new BsonDocument("properties",
                    new BsonDocument("$map",
                    new BsonDocument
                                {
                                    { "input", "$properties" },
                                    { "in",
                    new BsonDocument("$mergeObjects",
                    new BsonArray
                                        {
                                            "$$this",
                                            new BsonDocument("Values",
                                            new BsonDocument("$sortArray",
                                            new BsonDocument
                                                    {
                                                        { "input", "$$this.Values" },
                                                        { "sortBy", 1 }
                                                    }))
                                        }) }
                                }))),
                new BsonDocument("$set",
                new BsonDocument("powers",
                new BsonDocument("$getField",
                new BsonDocument
                            {
                                { "field", "Values" },
                                { "input",
                new BsonDocument("$first",
                new BsonDocument("$filter",
                new BsonDocument
                                        {
                                            { "input", "$properties" },
                                            { "cond",
                new BsonDocument("$eq",
                new BsonArray
                                                {
                                                    "$$this.Name",
                                                    "powers"
                                                }) }
                                        })) }
                            }))),
                new BsonDocument("$set",
                new BsonDocument("powersLength",
                new BsonDocument("$size",
                new BsonDocument("$ifNull",
                new BsonArray
                                {
                                    "$powers",
                                    new BsonArray()
                                })))),
                new BsonDocument("$sort",
                new BsonDocument
                    {
                        { "powersLength", -1 },
                        { "powers", 1 }
                    }),
                new BsonDocument("$unset",
                new BsonArray
                    {
                        "powers",
                        "powersLength"
                    })
            };

            return await _col.Aggregate<People>(pipeline)
                .ToListAsync();
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <param name="_col"></param>
        /// <returns></returns>
        private async Task<List<People>> SortPeopleWithFluentAggregate(IMongoCollection<People> _col)
        {
            var setStageFirst = new BsonDocument("$set",
                new BsonDocument("powers",
                    new BsonDocument("$getField",
                        new BsonDocument
                        {
                            { "field", "Values" },
                            { "input",
                                new BsonDocument("$first",
                                    new BsonDocument("$filter",
                                        new BsonDocument
                                        {
                                            { "input", "$properties" },
                                            { "cond",
                                                new BsonDocument("$eq",
                                                    new BsonArray
                                                    {
                                                        "$$this.Name",
                                                        "powers"
                                                    })
                                            }
                                        }))
                            }
                        })));

            var setStageSecond = new BsonDocument("$set",
                new BsonDocument("powers",
                    new BsonDocument("$getField",
                        new BsonDocument
                        {
                            { "field", "Values" },
                            { "input",
                                new BsonDocument("$first",
                                    new BsonDocument("$filter",
                                        new BsonDocument
                                        {
                                            { "input", "$properties" },
                                            { "cond",
                                                new BsonDocument("$eq",
                                                    new BsonArray
                                                    {
                                                        "$$this.Name",
                                                        "powers"
                                                    })
                                            }
                                        }))
                            }
                        })));

            var setStageThird = new BsonDocument("$set",
                new BsonDocument("powersLength",
                    new BsonDocument("$size",
                        new BsonDocument("$ifNull",
                            new BsonArray
                            {
                                "$powers",
                                new BsonArray()
                            }))));

            var sortStage = new BsonDocument
            {
                { "powersLength", -1 },
                { "powers", 1 }
            };

            var unsetStage = new BsonDocument("$unset",
                new BsonArray
                    {
                        "powers",
                        "powersLength"
                    });

            return await _col.Aggregate()
                .AppendStage<BsonDocument>(setStageFirst)
                .AppendStage<BsonDocument>(setStageSecond)
                .AppendStage<BsonDocument>(setStageThird)
                .Sort(sortStage)
                .AppendStage<People>(unsetStage)
                .ToListAsync();
        }

        public class People
        {
            [BsonId]
            public int Id { get; set; }

            [BsonElement("name")]
            public string Name { get; set; }

            [BsonElement("properties")]
            public List<Property> Properties { get; set; }
        }

        public class Property
        {
            public string Name { get; set; }
            public List<string> Values { get; set; }
        }
    }
}
