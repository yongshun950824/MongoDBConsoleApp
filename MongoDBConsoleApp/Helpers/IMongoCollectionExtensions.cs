using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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

            return query.Filter.Render(new RenderArgs<T>(
                collection.DocumentSerializer,
                collection.Settings.SerializerRegistry
                )).ToJson();
        }

        public static BsonDocument QueryToBson<T>(this IMongoCollection<T> collection,
            FilterDefinition<T> filter)
        {
            return filter.Render(new RenderArgs<T>(
                collection.DocumentSerializer,
                collection.Settings.SerializerRegistry));
        }

        /// <summary>
        /// <a href="https://stackoverflow.com/questions/79290997/how-to-call-render-method-of-filterdefinition-in-mongo-3-1-0-c-sharp-drivers/79293294#79293294">
        /// Question.
        /// </a>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static BsonDocument ToBsonDocument<T>(this FilterDefinition<T> filter)
        {
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<T>();
            return filter.Render(new RenderArgs<T>(documentSerializer, serializerRegistry));
        }
    }
}
