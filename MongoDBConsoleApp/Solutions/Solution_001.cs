using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/69079627/mongodb-net-driver-filter-builder-throwing-an-exception/69414324#69414324">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_001 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<Record> _collection = _database.GetCollection<Record>("record");

            InitData(_collection);

            var names = new List<Name>();
            names.Add(new Name { FirstName = "abc", LastName = "xyz" });
            names.Add(new Name { FirstName = "123", LastName = "789" });
            names.Add(new Name { FirstName = "a1b2", LastName = "c7d8" });

            List<FilterDefinition<Record>> namefilters = new List<FilterDefinition<Record>>();

            foreach (var name in names)
            {
                FilterDefinition<Record> namefilter = Builders<Record>.Filter.And(
                    Builders<Record>.Filter.Eq(x => x.FirstName, name.FirstName),
                    Builders<Record>.Filter.Eq(x => x.LastName, name.LastName)
                );

                namefilters.Add(namefilter);
            }

            FilterDefinition<Record> filter = Builders<Record>.Filter.Or(namefilters);

            var filteredRecords = _collection.Find(filter)
                .ToList();

            PrintOutput(filteredRecords);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        private void InitData(IMongoCollection<Record> _collection)
        {
            List<Record> recordsInserted = new List<Record>
            {
                new Record { FirstName = "abc", LastName = "xyz" },
                new Record { FirstName = "def", LastName = "ghi" },
                new Record { FirstName = "123", LastName = "789" },
                new Record { FirstName = "opq", LastName = "rst" },
                new Record { FirstName = "456", LastName = "654" }
            };

            _collection.InsertMany(recordsInserted);

            Thread.Sleep(2000);
        }

        private void PrintOutput(List<Record> filteredRecords)
        {
            Console.WriteLine(filteredRecords.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }

        class Record
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        class Name
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
