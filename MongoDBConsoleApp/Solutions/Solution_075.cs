using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/77493473/mongodb-query-to-transform-documents/77493707#77493707">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_075 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<TagCategoriesModel> _collection = _db.GetCollection<TagCategoriesModel>("tag_categories");

            var result = await _collection.Aggregate()
                .Unwind<TagCategoriesModel, UnwindTagCategoriesModel>(x => x.Categories)
                .Group(x => x.Categories,
                    g => new CategoryModel
                    {
                        Category = g.Key,
                        Tags = g.Select(y => new TagModel
                        {
                            Name = y.TagName,
                            Value = y.TagValue
                        })
                        .Distinct()
                        .ToList()
                    })
                .ToListAsync();

            Helpers.PrintFormattedJson(result);
        }
    }

    public class TagCategoriesModel
    {
        public ObjectId Id { get; set; }
        public string TagName { get; set; }
        public string TagValue { get; set; }
        public List<string> Categories { get; set; }
        public List<string> OtherField1 { get; set; }
        public string OtherField2 { get; set; }
    }

    public class UnwindTagCategoriesModel
    {
        public ObjectId Id { get; set; }
        public string TagName { get; set; }
        public string TagValue { get; set; }
        public string Categories { get; set; }
        public List<string> OtherField1 { get; set; }
        public string OtherField2 { get; set; }
    }

    [BsonNoId]
    public class CategoryModel
    {
        public string Category { get; set; }
        public List<TagModel> Tags { get; set; }
    }

    [BsonNoId]
    public class TagModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
