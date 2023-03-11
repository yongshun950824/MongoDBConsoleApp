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
    /// <a href="https://stackoverflow.com/questions/75696037/mongodb-net-driver-lookup-result-to-one-merged-grouped-array/75697095#75697095">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_065 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            this.RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<BsonDocument> _collection = _db.GetCollection<BsonDocument>("Portfolio");

            BsonDocument[] pipeline = new BsonDocument[]
            {
                new BsonDocument("$lookup",
                new BsonDocument
                    {
                        { "from", "Securities" },
                        { "localField", "Stocks.StockId" },
                        { "foreignField", "StockId" },
                        { "as", "Securities" }
                    }),
                new BsonDocument("$lookup",
                new BsonDocument
                    {
                        { "from", "MutualFunds" },
                        { "localField", "MutualFunds.MutualFundId" },
                        { "foreignField", "MfId" },
                        { "as", "Mfs" }
                    }),
                new BsonDocument("$set",
                new BsonDocument
                    {
                        { "Securities",
                new BsonDocument("$map",
                new BsonDocument
                            {
                                { "input", "$Securities" },
                                { "as", "s" },
                                { "in",
                new BsonDocument("$mergeObjects",
                new BsonArray
                                    {
                                        "$$s",
                                        new BsonDocument("$first",
                                        new BsonDocument("$filter",
                                        new BsonDocument
                                                {
                                                    { "input", "$Stocks" },
                                                    { "cond",
                                        new BsonDocument("$eq",
                                        new BsonArray
                                                        {
                                                            "$$s.StockId",
                                                            "$$this.StockId"
                                                        }) }
                                                }))
                                    }) }
                            }) },
                        { "Mfs",
                new BsonDocument("$map",
                new BsonDocument
                            {
                                { "input", "$Mfs" },
                                { "as", "m" },
                                { "in",
                new BsonDocument("$mergeObjects",
                new BsonArray
                                    {
                                        "$$m",
                                        new BsonDocument("$first",
                                        new BsonDocument("$filter",
                                        new BsonDocument
                                                {
                                                    { "input", "$MutualFunds" },
                                                    { "cond",
                                        new BsonDocument("$eq",
                                        new BsonArray
                                                        {
                                                            "$$m.MutualFundId",
                                                            "$$this.MfId"
                                                        }) }
                                                }))
                                    }) }
                            }) }
                    }),
                new BsonDocument("$unset",
                new BsonArray
                    {
                        "Stocks",
                        "MutualFunds",
                        "Securities._id",
                        "Mfs._id",
                        "Mfs.MutualFundId"
                    })
            };

            List<PortfolioOutputModel> result = (await _collection.AggregateAsync<PortfolioOutputModel>(pipeline))
                .ToList();

            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }
    }

    [BsonNoId]
    [BsonIgnoreExtraElements]
    public class PortfolioOutputModel
    {
        public string Name { get; set; }
        public List<Security> Securities { get; set; }
        public List<MutualFund> Mfs { get; set; }
    }

    public class Security
    {
        public long StockId { get; set; }
        public string Name { get; set; }
        public decimal LastPrice { get; set; }
        public decimal LastPriceChange { get; set; }
        public int Quantity { get; set; }
        public int InvestedValue { get; set; }
    }

    public class MutualFund
    {
        public long MfId { get; set; }
        public string Name { get; set; }
        public decimal LastNAV { get; set; }
        public decimal LastNAVChange { get; set; }
        public int Quantity { get; set; }
        public int InvestedValue { get; set; }
    }
}
