using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/71069862/c-sharp-mongodb-how-to-select-nested-properties-only/71077234#71077234">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_018 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            var _collection = _db.GetCollection<TestA>("testA");

            var result = await GetCustomerCommentsWithBsonDocument(_collection);

            PrintOutput(result);
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <param name="_collection"></param>
        /// <returns></returns>
        private async Task<List<CustomerComment>> GetCustomerCommentsWithAggregateFluent(
            IMongoCollection<TestA> _collection)
        {
            return await _collection.Aggregate()
                .Match(
                    p => p.ProjectID == "555"
                    && p.Content.CustInfo.Any(l => l.Id == "123")
                )
                .Unwind<TestA, UnwindTestA>(x => x.Content.CustInfo)
                .Match<UnwindTestA>(x => x.Content.CustInfo.Id == "123")
                .ReplaceWith<CustomerComment>("$Content.CustInfo.CustomerComment")
                .ToListAsync();
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <param name="_collection"></param>
        /// <returns></returns>
        private async Task<List<CustomerComment>> GetCustomerCommentsWithBsonDocument(
            IMongoCollection<TestA> _collection)
        {
            BsonDocument[] aggregate = new BsonDocument[]
            {
                new BsonDocument("$match",
                    new BsonDocument
                    {
                        { "ProjectID", "555" },
                        { "Content.CustInfo.Id", "123" }
                    }),
                new BsonDocument("$unwind", "$Content.CustInfo"),
                new BsonDocument("$match",
                    new BsonDocument("Content.CustInfo.Id",
                        new BsonDocument("$eq", "123"))),
                new BsonDocument("$replaceWith", "$Content.CustInfo.CustomerComment")
            };

            return await _collection.Aggregate<CustomerComment>(aggregate)
                .ToListAsync();
        }

        private void PrintOutput(List<CustomerComment> result)
        {
            Console.WriteLine(result.ToJson(
                new MongoDB.Bson.IO.JsonWriterSettings
                {
                    Indent = true
                }
            ));
        }

        class TestA
        {
            [BsonId]
            public ObjectId _id { get; set; }
            [BsonElement("ProjectID")]
            public string ProjectID { get; set; }
            [BsonElement("Content")]
            public TestB Content { get; set; }
        }

        [BsonNoId]
        class TestB
        {
            //[BsonId]

            [BsonElement("Id")]
            public string Id { get; set; }
            [BsonElement("CustInfo")]
            public List<TestC> CustInfo { get; set; }
        }

        class UnwindTestA
        {
            [BsonId]
            public ObjectId _id { get; set; }
            [BsonElement("ProjectID")]
            public string ProjectID { get; set; }
            [BsonElement("Content")]
            public UnwindTestB Content { get; set; }
        }

        [BsonNoId]
        [BsonIgnoreExtraElements]
        class UnwindTestB
        {
            [BsonElement("Id")]
            public string Id { get; set; }
            [BsonElement("CustInfo")]
            public TestC CustInfo { get; set; }
        }

        //[BsonIgnoreExtraElements]
        [BsonNoId]
        class TestC
        {
            [BsonElement("Id")]
            public string Id { get; set; }
            [BsonElement("CustomerComment")]
            public CustomerComment CustomerComment { get; set; }
        }

        [BsonIgnoreExtraElements]
        [BsonNoId]
        class CustomerComment
        {
            //[BsonId]

            [BsonElement("Id")]
            public string Id { get; set; }
            [BsonElement("notes")]
            public string notes { get; set; }
        }

    }
}
