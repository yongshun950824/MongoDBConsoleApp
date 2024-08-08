using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    internal class Solution_084 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            #region Solution 2
            //BsonClassMap.RegisterClassMap<Header>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.UnmapField(x => x.Id);

            //    cm.MapMember(x => x.Id)
            //        .SetElementName("id");
            //});
            #endregion

            var database = _client.GetDatabase("demo");
            var collection = database.GetCollection<Customer>("customer");

            var filter = Builders<Customer>.Filter.Eq("header.id", "0945f7cd-16a8-4ea6-b87f-c24e90dcfbc6");


            var customer = collection.Find(filter).FirstOrDefault();

            Helpers.PrintFormattedJson(customer);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        #region Solution 1
        [BsonNoId]
        #endregion
        public class Header
        {
            [BsonElement("id")]
            public string Id { get; set; }
        }

        public class Customer
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }

            [BsonElement("given_name")]
            public string GivenName { get; set; }

            [BsonElement("address_filled")]
            public bool AddressFilled { get; set; }

            [BsonElement("header")]
            public Header Header { get; set; }
        }
    }
}
