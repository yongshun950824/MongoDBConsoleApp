using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70928764/issue-with-data-return-in-mongodb/70934884#70934884">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_017 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            var _childDocument = _db.GetCollection<ChildDocument>("childDocument");
            var _masterDocument = _db.GetCollection<MasterDocument>("masterDocument");

            #region Solution 1
            //var result = GetResultWithLinq(_masterDocument, _childDocument);
            #endregion

            #region Solution 2
            //var result = GetResultWithBsonDocument(_masterDocument);
            #endregion

            #region Solution 3
            var result = GetResultWithAggregateFluent(_masterDocument);
            #endregion

            PrintOutput(result);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        /// <summary>
        /// Solution 1: Get result with LINQ
        /// </summary>
        /// <param name="_masterDocument"></param>
        /// <param name="_childDocument"></param>
        /// <returns></returns>
        private List<MasterDocument> GetResultWithLinq(IMongoCollection<MasterDocument> _masterDocument,
            IMongoCollection<ChildDocument> _childDocument)
        {
            return
                (from a in _masterDocument.AsQueryable<MasterDocument>()
                 join u in _childDocument.AsQueryable<ChildDocument>() on a.item equals u.sku
                 select new { master = a, child = u } into au
                 group au by au.master._id into masterGroup
                 select new MasterDocument()
                 {
                     _id = masterGroup.Key,
                     item = masterGroup.First().master.item,
                     price = masterGroup.First().master.price,
                     quantity = masterGroup.First().master.quantity,
                     childDocuments = masterGroup.Select(x => x.child).ToList()
                 })
                .ToList();
        }

        /// <summary>
        /// Solution 2: Get result with BsonDocument
        /// </summary>
        /// <param name="_masterDocument"></param>
        /// <returns></returns>
        private List<MasterDocument> GetResultWithBsonDocument(IMongoCollection<MasterDocument> _masterDocument)
        {
            PipelineDefinition<MasterDocument, MasterDocument> pipeline = new BsonDocument[]
            {
                new BsonDocument("$lookup",
                    new BsonDocument
                    {
                        { "from", "childDocument" },
                        { "localField", "item" },
                        { "foreignField", "sku" },
                        { "as", "data" }
                    })
            };

            return _masterDocument
                .Aggregate(pipeline)
                .ToList();
        }

        /// <summary>
        /// Solution 3: Get result with AggregateFluent
        /// </summary>
        /// <param name="_masterDocument"></param>
        /// <returns></returns>
        private List<MasterDocument> GetResultWithAggregateFluent(IMongoCollection<MasterDocument> _masterDocument)
        {
            return _masterDocument
                .Aggregate()
                .Lookup<ChildDocument, BsonDocument>(
                    "childDocument",
                    "item",
                    "sku",
                    "data")
                .Match(Builders<BsonDocument>.Filter.ElemMatch<BsonValue>("data", new BsonDocument("sku", "almonds")))
                .Project<MasterDocument>(Builders<BsonDocument>.Projection
                    .Exclude("data"))
                .ToList();
        }

        private void PrintOutput(List<MasterDocument> result)
        {
            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }

        class MasterDocument
        {
            public double _id { get; set; }
            public string item { get; set; }
            public double price { get; set; }
            public double quantity { get; set; }
            // For Solution 3
            [BsonElement("data")]
            public List<ChildDocument> childDocuments { get; set; }
        }

        class ChildDocument
        {
            public double _id { get; set; }
            public string sku { get; set; }
            public string description { get; set; }
            public int instock { get; set; }
        }
    }
}
