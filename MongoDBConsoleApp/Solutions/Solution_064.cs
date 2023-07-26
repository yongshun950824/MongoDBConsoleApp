using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/75659073/using-tolong-in-mongodb-c-sharp-queries/75659600#75659600">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_064 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            #region Solution 1
            List<Model> result = (await GetDataWithAggregationPipeline(_client))
                .ToList();
            #endregion

            #region Solution 2
            //List<Model> result = (await GetDataWithBsonSerializer(_client))
            //    .ToList();
            #endregion

            Helpers.PrintFormattedJson(result);
        }

        /// <summary>
        /// Solution 1.
        /// </summary>
        /// <param name="_client"></param>
        /// <returns></returns>
        private async Task<List<Model>> GetDataWithAggregationPipeline(IMongoClient _client)
        {
            IMongoCollection<Model> _collection = GetCollection(_client);

            var pipelineDefinition = new BsonDocument[]
            {
                new BsonDocument
                {
                    {
                        "$project", new BsonDocument
                        {
                            { "date", new BsonDocument { { "$toLong", "$date" } } },
                            { "value", 1 }
                        }
                    }
                }
            };

            return (await _collection.AggregateAsync<Model>(pipelineDefinition))
                .ToList();
        }

        /// <summary>
        /// Solution 2.
        /// </summary>
        /// <param name="_client"></param>
        /// <returns></returns>
        private async Task<List<Model>> GetDataWithBsonSerializer(IMongoClient _client)
        {
            BsonClassMap.RegisterClassMap<Model>(cm =>
            {
                cm.AutoMap();

                cm.MapMember(x => x.Date)
                    .SetSerializer(new DateToLongConverter());
            });

            IMongoCollection<Model> _collection = GetCollection(_client);

            return (await _collection.FindAsync(new BsonDocument()))
                .ToList();
        }

        private IMongoCollection<Model> GetCollection(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Model> _collection = _db.GetCollection<Model>("Solution_064");

            return _collection;
        }

        class Model
        {
            public ObjectId Id { get; set; }
            public DateTime Date { get; set; }
            public int Value { get; set; }
        }

        class DateToLongConverter : IBsonSerializer<long>
        {
            public Type ValueType => typeof(long);

            public long Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            {
                var bsonReader = context.Reader;
                return bsonReader.ReadDateTime();
            }

            object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            {
                var bsonReader = context.Reader;
                return bsonReader.ReadDateTime();
            }

            public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, long value)
            {
                var bsonWriter = context.Writer;
                bsonWriter.WriteInt64(value);
            }

            public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
            {
                var bsonWriter = context.Writer;
                bsonWriter.WriteInt64((long)value);
            }
        }
    }
}
