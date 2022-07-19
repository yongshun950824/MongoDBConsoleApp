using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static MongoDBConsoleApp.Program;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/72794815/how-can-i-materialize-a-string-to-the-actual-type/72795479#72795479">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_027 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            var _collection = _db.GetCollection<masterDocument>("masterDocument");

            string fieldName = "item";

            var result = _collection.Find(x => true)
                .SortByDescending(ToSortByExpression<masterDocument>(fieldName))
                .ToList();

            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private static Expression<Func<T, Object>> ToSortByExpression<T>(string propertyName) where T : class
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Property Name annot be null or empty.");

            System.Reflection.PropertyInfo prop = typeof(T).GetProperty(propertyName);
            if (prop == null)
                throw new ArgumentException("Property name is not existed.");

            // Create ParameterExpression
            ParameterExpression param = Expression.Parameter(typeof(T), "x");

            // Create MemberExpression
            MemberExpression member = Expression.Property(param, propertyName);

            return Expression.Lambda<Func<T, Object>>(member, new[] { param });
        }

        class masterDocument
        {
            public double _id { get; set; }
            public string item { get; set; }
            public double price { get; set; }
            public double quantity { get; set; }
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
