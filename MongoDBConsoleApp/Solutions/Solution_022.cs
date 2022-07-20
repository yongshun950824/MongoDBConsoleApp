﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/71579890/mongodb-c-sharp-driver-nested-lookups-how-do-i-join-nested-relations/71582556#71582556">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_022 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");

            var pipeline = new[]
            {
                new BsonDocument("$lookup",
                    new BsonDocument
                    {
                        { "from", "_Store" },
                        { "let",
                            new BsonDocument("stores", "$stores")
                        },
                        { "pipeline",
                            new BsonArray
                            {
                                new BsonDocument("$match",
                                    new BsonDocument("$expr",
                                        new BsonDocument("$in",
                                            new BsonArray
                                            {
                                                "$_id",
                                                "$$stores"
                                            }
                                        )
                                    )
                                ),
                                new BsonDocument("$lookup",
                                    new BsonDocument
                                    {
                                        { "from", "_Product" },
                                        { "let",
                                            new BsonDocument("products", "$products")
                                        },
                                        { "pipeline",
                                            new BsonArray
                                            {
                                                new BsonDocument("$match",
                                                    new BsonDocument("$expr",
                                                        new BsonDocument("$in",
                                                            new BsonArray
                                                            {
                                                                "$_id",
                                                                "$$products"
                                                            }
                                                        )
                                                    )
                                                )
                                            }
                                        },
                                        { "as", "products" }
                                    }
                                )
                            }
                        },
                        { "as", "stores" }
                    }
                )
            };

            var result = _db.GetCollection<BsonDocument>("_Company")
                .Aggregate<BsonDocument>(pipeline)
                .ToList();

            PrintOutput(result);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private void PrintOutput(List<BsonDocument> result)
        {
            Console.WriteLine(result.ToJson(new MongoDB.Bson.IO.JsonWriterSettings
            {
                Indent = true
            }));
        }
    }
}