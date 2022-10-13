using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73738948/net-core-mongodb-driver-objectid-null-from-poco-mapping?r=SearchResults&s=1%7C43.6517">
    /// Question
    /// </a>
    /// </summary>
    internal class Solution_045 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            BsonClassMap.RegisterClassMap<RR4NFAD>(cm =>
            {
                cm.AutoMap();
                cm.UnmapProperty(c => c.Id);
                cm.MapMember(c => c.Id)
                    .SetElementName("$id");
            });

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Root> _colllection = _db.GetCollection<Root>("Solution_045");

            var result = await _colllection.Find(Builders<Root>.Filter.Empty)
                .ToListAsync();

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }

    [BsonNoId]
    [BsonIgnoreExtraElements]
    public class Root
    {
        public List<Lrec80> lrec80 { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Lrec80
    {
        public RR4NFAD RR4NFAD { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class RR4NFAD
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("$ref")]
        public string Ref { get; set; }

        [BsonElement("$db")]
        public string DB { get; set; }
    }
}
