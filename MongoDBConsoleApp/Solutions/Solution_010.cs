﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70729292/filter-ltex-x-price-9-getting-wrong-results/70729526#70729526">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_010 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var collection = _database.GetCollection<Product>("product");

            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Empty;
            filterDefinition &= new BsonDocument("$expr",
                new BsonDocument("$lte",
                    new BsonArray
                    {
                        new BsonDocument("$toInt", "$price"),
                        9
                    }));

            var result = collection.Find(filterDefinition)
                .ToList();

            PrintOutput(result);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private void PrintOutput(List<Product> result)
        {
            foreach (var item in result)
            {
                Console.WriteLine(item.ToJson());
            }
        }

        class Product
        {
            public ObjectId Id { get; set; }
            public string name { get; set; }
            public string price { get; set; }
        }
    }
}