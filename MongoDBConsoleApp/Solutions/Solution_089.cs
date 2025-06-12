using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/77728716/how-to-aggregate-mongodb-collections-using-c-sharp-classes/77730452#77730452">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_089 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase Database = _client.GetDatabase("demo");

            IMongoCollection<TeamData> teamCollection = Database.GetCollection<TeamData>("Teams");
            IMongoCollection<League> leagues = Database.GetCollection<League>("Leagues");
            IMongoCollection<Player> players = Database.GetCollection<Player>("Players");

            var data = (from team in teamCollection.AsQueryable()
                        join league in leagues.AsQueryable() on team.LeagueId equals league.Id
                        join player1 in players.AsQueryable() on team.Player1Id equals player1.Id
                        join player2 in players.AsQueryable() on team.Player2Id equals player2.Id
                        select new Team
                        {
                            Id = team.Id,
                            League = league,
                            Number = team.Number,
                            Player1 = player1,
                            Player2 = player2,
                        }).ToList();

            data = await teamCollection.Aggregate<Team>(new BsonDocument[]
                {
                    new BsonDocument("$lookup",
                        new BsonDocument
                            {
                                { "from", "Leagues" },
                                { "localField", "LeagueId" },
                                { "foreignField", "_id" },
                                { "as", "Leagues" }
                            }),
                    new BsonDocument("$lookup",
                        new BsonDocument
                            {
                                { "from", "Players" },
                                { "localField", "Player1Id" },
                                { "foreignField", "_id" },
                                { "as", "PlayerOnes" }
                            }),
                    new BsonDocument("$lookup",
                        new BsonDocument
                            {
                                { "from", "Players" },
                                { "localField", "Player2Id" },
                                { "foreignField", "_id" },
                                { "as", "PlayerTwos" }
                            }),
                    new BsonDocument("$project",
                        new BsonDocument
                        {
                            { "_id", 1 },
                            { "Number", 1 },
                            { "League",
                                new BsonDocument("$first", "$Leagues") },
                            { "Player1",
                                new BsonDocument("$first", "$PlayerOnes") },
                            { "Player2",
                                new BsonDocument("$first", "$PlayerTwos") }
                        })
                }).ToListAsync();

            Helpers.PrintFormattedJson(data);
        }
    }

    public class Player
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Handicap { get; set; }
    }

    public class League
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class Team
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Number { get; set; }

        public League League { get; set; }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }
    }

    public class TeamData
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Number { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string LeagueId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Player1Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Player2Id { get; set; }
    }
}
