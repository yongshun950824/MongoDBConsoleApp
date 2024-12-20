using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/76782517/serialization-inside-mongodb-driver-filter/76784612#76784612">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_071 : ISolution
    {
        // TO-DO: Initialize with constructor (to work as dependency injection)
        private IMongoDatabase _database;
        private IMongoCollection<CloudFile> _fileCollection;

        public void Run(IMongoClient _client)
        {
            BsonSerializer.RegisterSerializer(typeof(CloudFileTypes), new EnumMemberStringSerializer<CloudFileTypes>());

            _database = _client.GetDatabase("demo");
            _fileCollection = _database.GetCollection<CloudFile>("CloudFile");

            CloudFile cloudFile = GetRoot();

            Console.WriteLine("Result:");
            Helpers.PrintFormattedJson(cloudFile);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        private CloudFile GetRoot(/*User user*/)
        {
            var builder = Builders<CloudFile>.Filter;
            var query = builder.Eq(e => e.Type, CloudFileTypes.Folder);

            var root = _fileCollection.Find(query).FirstOrDefault();

            Console.WriteLine("Query:");
            Helpers.PrintFormattedJson(_fileCollection.QueryToBson(query));

            return root;
        }

        class CloudFile
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }
            [BsonElement("name")] public string Name { get; set; }
            //[JsonConverter(typeof(JsonStringEnumConverter))]
            [JsonConverter(typeof(StringEnumConverter))]
            [BsonElement("type")] public CloudFileTypes Type { get; set; }
            [BsonElement("size")] public int Size { get; set; }
            [BsonElement("path")] public string Path { get; set; }
            [BsonRepresentation(BsonType.ObjectId)]
            [BsonElement("user")] public string User { get; set; }
            [BsonRepresentation(BsonType.ObjectId)]
            [BsonElement("parent")] public string Parent { get; set; }
            [BsonRepresentation(BsonType.ObjectId)]
            [BsonElement("childs")] public string[] Childs { get; set; }
        }

        enum CloudFileTypes
        {
            [EnumMember(Value = "root")] Root,
            [EnumMember(Value = "folder")] Folder,
            [EnumMember(Value = "file")] File,
        }

        class EnumMemberStringSerializer<TEnum> : IBsonSerializer<TEnum>
            where TEnum : struct, Enum
        {
            public new Type ValueType => typeof(TEnum);

            public TEnum Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            {
                return EnumExtensions.EnumMemberValueToEnum<TEnum>(context.Reader.ReadString());
            }

            public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEnum value)
            {
                context.Writer.WriteString(EnumExtensions.GetEnumMemberValue((TEnum)value));
            }

            public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
            {
                context.Writer.WriteString(EnumExtensions.GetEnumMemberValue((TEnum)value));
            }

            object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            {
                return EnumExtensions.EnumMemberValueToEnum<TEnum>(context.Reader.ReadString());
            }
        }
    }
}
