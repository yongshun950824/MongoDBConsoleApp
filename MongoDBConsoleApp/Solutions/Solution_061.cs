using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/75212463/mongodb-net-driver-how-to-increment-double-nested-field/75229701#75229701">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_061 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            this.RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Guid competitionId = Guid.Parse("b350c9fd-3632-4b0a-b5cb-66e41d530f55");
            Guid сoefficientId = Guid.Parse("b350c9fd-3632-4b0a-b5cb-66e41d530f55");
            int amount = 10;

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<CompetitionDota2Entity> _collection = _db.GetCollection<CompetitionDota2Entity>("competition");

            var competitionIdstring = competitionId.ToString();
            var сoefficientIdstring = сoefficientId.ToString();
            CancellationToken token = new CancellationToken();

            var filterBuilder = Builders<CompetitionDota2Entity>.Filter;
            var updateBuilder = Builders<CompetitionDota2Entity>.Update;

            var filterCompetitionId = filterBuilder.Eq(x => x.Id, competitionId.ToString());

            #region Solution for MongoDB .NET Driver v2.16 and above
            var update = Builders<CompetitionDota2Entity>.Update;
            var incAmount = update.Inc(x => x.CoefficientGroups.AllMatchingElements("cg")
                .Coefficients.AllMatchingElements("c").Amount,
                amount);
            #endregion

            #region Solution for MongoDB .NET Driver v2.16 before
            //var incAmount = update.Inc(
            //    "coefficient_groups.$[cg].coefficients.$[c].amount",
            //    amount);
            #endregion

            FindOneAndUpdateOptions<CompetitionDota2Entity> _defaultCompetitionDota2EntityFindOption = new FindOneAndUpdateOptions<CompetitionDota2Entity>
            {
                ArrayFilters = new ArrayFilterDefinition[]
                {
                    new BsonDocumentArrayFilterDefinition<CoefficientGroup>
                    (
                        new BsonDocument("cg._id", competitionIdstring)
                    ),
                    new BsonDocumentArrayFilterDefinition<Coefficient>
                    (
                        new BsonDocument("c._id", сoefficientIdstring)
                    )
                }
            };

            var existingCoefficientGroup = await _collection.FindOneAndUpdateAsync(
                filterCompetitionId,
                incAmount,
                _defaultCompetitionDota2EntityFindOption,
                token);

            Console.WriteLine(existingCoefficientGroup.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }

        public class CompetitionDota2Entity
        {
            [BsonElement("id")]
            public string Id { get; set; }

            [BsonElement("type")]
            public string Type { get; set; }

            [BsonElement("status_type")]
            public string StatusType { get; set; }

            [BsonElement("start_time")]
            public StartTime StartTime { get; set; }

            [BsonElement("coefficient_groups")]
            public List<CoefficientGroup> CoefficientGroups { get; set; }

            [BsonElement("team1_id")]
            public string Team1Id { get; set; }

            [BsonElement("team2_id")]
            public string Team2Id { get; set; }

            [BsonElement("team1_kill_amount")]
            public int Team1KillAmount { get; set; }

            [BsonElement("team2_kill_amount")]
            public int Team2KillAmount { get; set; }

            [BsonElement("total_time")]
            public TotalTime TotalTime { get; set; }
        }

        public class Coefficient
        {
            [BsonElement("id")]
            public string Id { get; set; }

            [BsonElement("description")]
            public string Description { get; set; }

            [BsonElement("rate")]
            public double Rate { get; set; }

            [BsonElement("status_type")]
            public string StatusType { get; set; }

            [BsonElement("amount")]
            public int Amount { get; set; }

            [BsonElement("probability")]
            public int Probability { get; set; }
        }

        public class CoefficientGroup
        {
            [BsonElement("id")]
            public string Id { get; set; }

            [BsonElement("name")]
            public string Name { get; set; }

            [BsonElement("type")]
            public string Type { get; set; }

            [BsonElement("coefficients")]
            public List<Coefficient> Coefficients { get; set; }
        }



        public class StartTime
        {
            [BsonElement("seconds")]
            public string Seconds { get; set; }

            [BsonElement("nanos")]
            public int Nanos { get; set; }
        }

        public class TotalTime
        {
            [BsonElement("seconds")]
            public string Seconds { get; set; }

            [BsonElement("nanos")]
            public int Nanos { get; set; }
        }
    }
}
