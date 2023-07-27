using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    internal class Solution_070 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<App> _collection = _database.GetCollection<App>("app");

            string group = "12345";

            var applicationFilter = Builders<App>.Filter.Eq(a => a.IsDeleted, false) &
                Builders<App>.Filter.Exists(a => a.Versions, true) &
                Builders<App>.Filter.SizeGt(a => a.Versions, 0);

            #region Solution 1
            //applicationFilter &= new BsonDocument("$expr",
            //    new BsonDocument("$ne",
            //        new BsonArray
            //        {
            //            new BsonDocument("$last", "$versions.group"),
            //            group
            //        }));
            #endregion

            #region Solution 2
            // Require enable `LinqProvider = MongoDB.Driver.Linq.LinqProvider.V3` in MongoSettings
            applicationFilter &= Builders<App>.Filter.Where(x => x.Versions.Last().Group != group);
            #endregion

            Helpers.PrintFormattedJson((await _collection.FindAsync(applicationFilter))
                .ToList());
        }

        class App
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public List<Version> Versions { get; set; } = new List<Version>();

            public bool IsDeleted { get; set; }
        }

        class Version
        {
            public DateTime DeployedDate { get; set; }

            public string DeploymentId { get; set; }

            public string Group { get; set; }
        }
    }
}
