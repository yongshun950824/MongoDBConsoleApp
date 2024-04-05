using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDBConsoleApp.Conventions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MongoDBConsoleApp
{
    public static class Helpers
    {
        public static void RegisterCamelCasePack()
        {
            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("camel case", pack, t => true);
        }

        public static void PrintFormattedJson<T>(this T result)
        {
            if (result is BsonDocument
                || result is BsonDocument[]
                || result is IEnumerable<BsonDocument>
                || result is BsonArray)
            {
                Console.WriteLine(result.ToJson(new MongoDB.Bson.IO.JsonWriterSettings
                {
                    Indent = true
                }));
            }
            else
            {
                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }
        }

        public static void MongoDbJsonPropertyConvention()
        {
            var pack = new ConventionPack();
            pack.Add(new MongoDbJsonPropertyConvention());
            ConventionRegistry.Register("Read JsonProperty Attribute", pack, t => true);
        }
    }
}
