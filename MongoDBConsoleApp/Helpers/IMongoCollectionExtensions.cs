using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace MongoDBConsoleApp
{
    public static class IMongoCollectionExtensions
    {
        public static string ExpressionToJson<T>(this IMongoCollection<T> collection,
            Expression<Func<T, bool>> filter)
        {
            var query = collection.Find(filter);

            return query.Filter.Render(
                collection.DocumentSerializer,
                collection.Settings.SerializerRegistry
                ).ToJson();
        }

        public static BsonDocument QueryToBson<T>(this IMongoCollection<T> collection,
            FilterDefinition<T> filter)
        {
            return filter.Render(
                collection.DocumentSerializer,
                collection.Settings.SerializerRegistry);
        }
    }
}
