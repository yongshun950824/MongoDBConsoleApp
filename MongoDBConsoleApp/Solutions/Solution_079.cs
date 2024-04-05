using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/78272913/mongodb-net-driver-update-field-in-nested-sub-array-document/78274046#78274046">
    /// Question.
    /// </a>
    /// </summary>
    public class Solution_079 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

            IMongoDatabase database = _client.GetDatabase("demo");

            //await InsertProductAsync(database);

            Guid productId = new Guid("f599b3fa-4c10-42ae-b9cd-b329aafa266b");
            Guid productVariantId = new Guid("a67073af-68a6-4b5d-89da-1736b1def4e0");

            bool result = await UpdateImageAssociationAsync(database,
                productId,
                productVariantId,
                Guid.NewGuid(),
                "New image");

            Console.WriteLine("Is Updated: " + result);

            var filter = Builders<Product>.Filter
                .Where(x => x.Id == productId
                    && x.Variants.Any(pv => pv.Id == productVariantId));

            var productsCollection = database.GetCollection<Product>(nameof(Product));

            var res = (await productsCollection.FindAsync(filter)).FirstOrDefault();
            Helpers.PrintFormattedJson(res);
        }

        private async Task InsertProductAsync(IMongoDatabase database)
        {
            var newProduct = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(@"
            {
                ""Id"" : ""f599b3fa-4c10-42ae-b9cd-b329aafa266b"",
                ""Title"" : ""Prod 1"",
                ""Variants"" : [ 
                    {
                        ""Id"" : ""a67073af-68a6-4b5d-89da-1736b1def4e0"",
                        ""CategoryImageId"" : ""00000000-0000-0000-0000-000000000000"",
                        ""ImageName"" : null
                    }, 
                    {
                        ""Id"" : ""a04cb54e-d171-4a00-8c29-8e55b930ced0"",
                        ""CategoryImageId"" : ""00000000-0000-0000-0000-000000000000"",
                        ""ImageName"" : null
                    }
                ]
            }");

            var productsCollection = database.GetCollection<Product>(nameof(Product));
            await productsCollection.InsertOneAsync(newProduct);
        }

        private async Task<bool> UpdateImageAssociationAsync(IMongoDatabase database,
            Guid productId,
            Guid productVariantId,
            Guid categoryImageId,
            string imageName)
        {
            try
            {
                var productsCollection = database.GetCollection<Product>(nameof(Product));
                if (productsCollection == null)
                    return false;

                var filter = Builders<Product>.Filter.Where(x => x.Id == productId
                    && x.Variants.Any(pv => pv.Id == productVariantId));
                var updateFilter = Builders<Product>.Update
                    .Set("Variants.$[variant].CategoryImageId", categoryImageId)
                    .Set("Variants.$[variant].ImageName", imageName);

                var res = await productsCollection.UpdateOneAsync(filter,
                    updateFilter,
                    new UpdateOptions
                    {
                        ArrayFilters = new List<ArrayFilterDefinition<Variant>>
                        {
                            new BsonDocumentArrayFilterDefinition<Variant>(
                                new BsonDocument("variant._id", BsonValue.Create(productVariantId))
                            )
                        }
                    });

                return res.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal class Product
        {
            [BsonElement("_id")]
            [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
            public Guid Id { get; set; }
            public string Title { get; set; }
            public List<Variant> Variants { get; set; }
        }

        internal class Variant
        {
            [BsonElement("_id")]
            [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
            public Guid Id { get; set; }
            [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
            public Guid CategoryImageId { get; set; }
            public object ImageName { get; set; }
        }
    }
}
