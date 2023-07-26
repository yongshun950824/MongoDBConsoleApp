using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/75259584/how-to-filter-a-collection-for-nested-objects-with-different-types/75280593#75280593">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_063 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<BaseVehicle> _collection = _db.GetCollection<BaseVehicle>("vehicle");

            #region Solution 1
            var result = await _collection.Find(GetQueryWithFluent(_db))
                .ToListAsync();
            #endregion

            #region Solution 2
            //var result = await _collection.Find(GetQueryWithBsonDocument())
            //    .ToListAsync();
            #endregion

            Helpers.PrintFormattedJson(result);
        }

        /// <summary>
        /// Solution 1: Query via Fluent API to BsonDocument
        /// </summary>
        /// <returns></returns>
        private BsonDocument GetQueryWithFluent(IMongoDatabase _db)
        {
            IMongoCollection<BaseVehicle> _collection = _db.GetCollection<BaseVehicle>("vehicle");
            IMongoCollection<Suv> _suvCollection = _db.GetCollection<Suv>("vehicle");
            IMongoCollection<Truck> _truckCollection = _db.GetCollection<Truck>("vehicle");
            IMongoCollection<BsonDocument> _bsonCollection = _db.GetCollection<BsonDocument>("vehicle");

            FilterDefinition<Suv> suvFilter = Builders<Suv>.Filter.ElemMatch(x => x.Engines,
                Builders<Engine>.Filter.Eq(y => y.HorsePower, 1000));

            FilterDefinition<Truck> truckFilter = Builders<Truck>.Filter.ElemMatch(x => x.Parts,
                Builders<TruckPart>.Filter.ElemMatch(y => y.Engines,
                    Builders<Engine>.Filter.Eq(z => z.HorsePower, 1000)));

            FilterDefinition<BaseVehicle> baseFilter = Builders<BaseVehicle>.Filter.Eq(x => x.YearOfProduction, 2022);

            FilterDefinition<BsonDocument> filter = _collection.QueryToBson(baseFilter);
            filter &= (FilterDefinition<BsonDocument>)_suvCollection.QueryToBson(suvFilter)
                | (FilterDefinition<BsonDocument>)_truckCollection.QueryToBson(truckFilter);

            return _bsonCollection.QueryToBson(filter);
        }

        /// <summary>
        /// Solution 2: Query via BsonDocument
        /// </summary>
        /// <returns></returns>
        private FilterDefinition<BaseVehicle> GetQueryWithBsonDocument()
        {
            FilterDefinition<BaseVehicle> filter = new BsonDocument
            {
                { "YearOfProduction", 2022 },
                { "$or", new BsonArray
                    {
                        new BsonDocument("Engines",
                            new BsonDocument("$elemMatch",
                                new BsonDocument("HorsePower", 1000))),
                        new BsonDocument("Parts",
                            new BsonDocument("$elemMatch",
                                new BsonDocument("Engines",
                                    new BsonDocument("$elemMatch",
                                        new BsonDocument("HorsePower", 1000)))))
                    }
                }
            };

            return filter;
        }

        [BsonDiscriminator(RootClass = true)]
        [BsonKnownTypes(typeof(Suv), typeof(Truck))]
        abstract class BaseVehicle
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int YearOfProduction { get; set; }
        }

        class Suv : BaseVehicle
        {
            public List<Engine> Engines { get; set; }
        }

        class Truck : BaseVehicle
        {
            public List<TruckPart> Parts { get; set; }
        }

        class TruckPart
        {
            public List<Engine> Engines { get; set; }
        }

        class Engine
        {
            public int HorsePower { get; set; }
        }
    }
}
