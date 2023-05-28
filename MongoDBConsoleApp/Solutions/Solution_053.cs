using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73203113/mongodb-net-driver-how-to-access-nested-element/74600693#74600693">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_053 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            this.RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            #region Solution 1: Class Map
            BsonClassMap.RegisterClassMap<DocumentType>(cm =>
            {
                cm.AutoMap();
                cm.UnmapField(x => x.id);

                cm.MapMember(x => x.id)
                    .SetElementName("id");
            });

            BsonClassMap.RegisterClassMap<DocumentTheme>(cm =>
            {
                cm.AutoMap();
                cm.UnmapField(x => x.id);

                cm.MapMember(x => x.id)
                    .SetElementName("id");
            });
            #endregion

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<DocumentFields> _collection = _db.GetCollection<DocumentFields>("document_fields");

            FilterDefinition<DocumentFields> filter = FilterDefinition<DocumentFields>.Empty;

            List<DocumentFields> documentFields = (await _collection.FindAsync(filter))
                .ToList();

            Console.WriteLine(JsonConvert.SerializeObject(documentFields, Formatting.Indented));
        }

        public class DocumentFields
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }

            [BsonElement("account_id")]
            public string AccountId { get; set; }

            [BsonElement("document_types")]
            public DocumentType[] DocumentTypes { get; set; }

            [BsonElement("document_themes")]
            public DocumentTheme[] DocumentThemes { get; set; }

            [BsonElement("created_at")]
            public DateTime CreatedAt { get; set; }
        }

        [BsonNoId]
        public class DocumentType
        {
            [BsonElement("id")]
            public string id { get; set; }

            [BsonElement("name")]
            public string name { get; set; }
        }

        [BsonNoId]
        public class DocumentTheme
        {
            [BsonElement("id")]
            public string id { get; set; }

            [BsonElement("name")]
            public string name { get; set; }
        }
    }
}
