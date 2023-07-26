using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/74909307/need-help-filter-list-of-array-based-on-specific-item-id-using-mongodb-c-sharp/74917629#74917629">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_056 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            int ItemId = 2;
            int sizeId = 2;

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Item> _collection = _db.GetCollection<Item>("item");

            var filter = Builders<Item>.Filter.Eq(x => x.Itemid, ItemId);
            var filter2 = Builders<Item>.Filter.ElemMatch(x => x.itemLists,
                Builders<ItemList>.Filter.Eq(x => x.Sizeid, sizeId));

            ProjectionDefinition<Item> projection = Builders<Item>.Projection
                .Include(x => x.Itemid)
                .Include(x => x.ItemName)
                .Include(x => x.ItemDescripton)
                .ElemMatch(x => x.itemLists, Builders<ItemList>.Filter.Eq(y => y.Sizeid, sizeId));

            var data = await _collection.Find(filter & filter2)
                .Project<Item>(projection)
                .FirstOrDefaultAsync();

            Helpers.PrintFormattedJson(data);
        }


        class ItemList
        {
            public int CategoryId { get; set; }
            public string CategpryName { get; set; }
            public int ColorId { get; set; }
            public string ColorName { get; set; }
            public int InitialQty { get; set; }
            public int AvailableQty { get; set; }
            public int ReserveQty { get; set; }
            public int Price { get; set; }
            public int OfferPrice { get; set; }
            public List<string> Images { get; set; }
            public int Sizeid { get; set; }
            public string SizeName { get; set; }
            public int DetailId { get; set; }
            public int DeliveryCharges { get; set; }
            public string Brand { get; set; }
            public DateTime CreatedOn { get; set; }
            public string CreatedBy { get; set; }
            public bool Active { get; set; }
        }

        class Item
        {
            [BsonId]
            [BsonElement("_id")]
            public int Itemid { get; set; }
            public string ItemName { get; set; }
            public string ItemDescripton { get; set; }
            public List<ItemList> itemLists { get; set; }
        }
    }
}
