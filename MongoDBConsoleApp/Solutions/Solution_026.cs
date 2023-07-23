using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/72538606/how-to-perform-like-on-mongodb-document-for-integer-values/72540375#72540375">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_026 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            var collection = _db.GetCollection<Book>("book");

            string searchString = "";
            string year = "98";
            FilterDefinition<Book> filterDefinition = Builders<Book>.Filter.Empty;
            filterDefinition &= Builders<Book>.Filter.Regex("subject", new BsonRegularExpression(searchString.ToString(), "i"));

            filterDefinition &= new BsonDocument("$expr",
                new BsonDocument("$regexMatch",
                    new BsonDocument
                    {
                        { "input", new BsonDocument("$toString", "$year") },
                        { "regex", year },
                        { "options", "i" }
                    }
                )
            );

            var result = await collection.Find(filterDefinition)
                .ToListAsync();

            PrintOutput(result);
        }

        private void PrintOutput(List<Book> result)
        {
            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }

        class Book
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public string Subject { get; set; }
            public string Name { get; set; }
            public int Year { get; set; }
        }
    }
}
