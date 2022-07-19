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
    /// <a href="https://stackoverflow.com/questions/70928764/issue-with-data-return-in-mongodb/70934884#70934884">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_017 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            var _childDocument = _db.GetCollection<childDocument>("childDocument");
            var _masterDocument = _db.GetCollection<masterDocument>("masterDocument");

            var result = Solution2(_masterDocument);
            PrintOutput(result);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private List<masterDocument> Solution1(IMongoCollection<masterDocument> _masterDocument)
        {
            PipelineDefinition<masterDocument, masterDocument> pipeline = new BsonDocument[]
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

        private List<masterDocument> Solution2(IMongoCollection<masterDocument> _masterDocument)
        {
            return _masterDocument
                .Aggregate()
                .Lookup<childDocument, BsonDocument>(
                    "childDocument",
                    "item",
                    "sku",
                    "data")
                .Match(Builders<BsonDocument>.Filter.ElemMatch<BsonValue>("data", new BsonDocument("sku", "almonds")))
                .Project<masterDocument>(Builders<BsonDocument>.Projection
                    .Exclude("data"))
                .ToList();
        }

        private List<masterDocument> Solution3(IMongoCollection<masterDocument> _masterDocument,
            IMongoCollection<childDocument> _childDocument)
        {
            return
                (from a in _masterDocument.AsQueryable<masterDocument>()
                 join u in _childDocument.AsQueryable<childDocument>() on a.item equals u.sku
                 select new { master = a, child = u } into au
                 group au by au.master._id into masterGroup
                 select new masterDocument()
                 {
                     _id = masterGroup.Key,
                     item = masterGroup.First().master.item,
                     price = masterGroup.First().master.price,
                     quantity = masterGroup.First().master.quantity,
                     childDocuments = masterGroup.Select(x => x.child).ToList()
                 })
                .ToList();
        }

        private void PrintOutput(List<masterDocument> result)
        {
            Console.WriteLine(result.ToJson(new MongoDB.Bson.IO.JsonWriterSettings
            {
                Indent = true
            }));
        }

        class masterDocument
        {
            public double _id { get; set; }
            public string item { get; set; }
            public double price { get; set; }
            public double quantity { get; set; }
            // For Solution 3
            [BsonElement("data")]
            public List<childDocument> childDocuments { get; set; }
        }

        class childDocument
        {
            public double _id { get; set; }
            public string sku { get; set; }
            public string description { get; set; }
            public int instock { get; set; }
        }
    }
}
