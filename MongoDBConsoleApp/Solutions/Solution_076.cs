using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/77555006/c-sharp-problem-reading-mongodb-data-as-name-value/77555092#77555092">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_076 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Root> _collection = _db.GetCollection<Root>("Solution_076");

            var result = await _collection.Find(Builders<Root>.Filter.Empty)
                .ToListAsync();

            Helpers.PrintFormattedJson(result);
        }
    }

    [BsonNoId]
    internal class Goods
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public int Price { get; set; }

        [BsonElement("number")]
        public int Number { get; set; }

        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("cate_id")]
        public int CateId { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("use_property")]
        public int UseProperty { get; set; }

        [BsonElement("props_text")]
        public string PropsText { get; set; }

        [BsonElement("props")]
        public List<int> Props { get; set; }
    }

    internal class Root
    {
        public ObjectId Id { get; set; }

        [BsonElement("StatusText")]
        public string StatusText { get; set; }

        [BsonElement("GoodsList")]
        public List<Goods> GoodsList { get; set; }

        [BsonElement("Notes")]
        public string Notes { get; set; }
    }
}
