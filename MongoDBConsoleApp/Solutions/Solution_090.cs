using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/79786685/mongodb-net-updating-embedded-document-in-list-with-filters-based-on-parent-a/79786923#79786923">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_090 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");

            var _collection = CreateTheDocs(_db);

            var filterTeam = Builders<Team>.Filter.Eq("TeamName", "GoldDiggers");
            var filterPlayer = Builders<Player>.Filter.Eq("PlayerName", "Greg");
            var filterTeamPlayers = Builders<Team>.Filter.ElemMatch(x => x.Players,
                filterPlayer);

            var combinedFilter = filterTeam & filterTeamPlayers;

            List<string> newColors = new List<string>() { "peach", "periwinkle" };

            UpdateDefinition<Team> updateDefinition = Builders<Team>.Update
                .Set(doc => doc.Players.AllMatchingElements("p").PlayerColors, newColors);

            var updateResult = await _collection.UpdateOneAsync(combinedFilter, updateDefinition,
                new UpdateOptions
                {
                    ArrayFilters = new ArrayFilterDefinition[]
                    {
                        new BsonDocumentArrayFilterDefinition<Player>
                        (
                            new BsonDocument("p.PlayerName", "Greg")
                        )
                    }
                });

            Helpers.PrintFormattedJson(updateResult);
        }

        private static IMongoCollection<Team> CreateTheDocs(IMongoDatabase db)
        {
            db.CreateCollection("Teams");
            IMongoCollection<Team> TeamsCollection = db.GetCollection<Team>("Teams");

            Team teamDoc = new Team() { TeamName = "SandPipers", TeamCode = 5567 };

            Player playerDoc = new Player() { PlayerName = "Suzie" };
            playerDoc.AddColor("black");
            playerDoc.AddColor("blue");
            teamDoc.AddPlayer(playerDoc);

            playerDoc = new Player() { PlayerName = "Sandy" };
            playerDoc.AddColor("brown");
            playerDoc.AddColor("beige");
            teamDoc.AddPlayer(playerDoc);

            playerDoc = new Player() { PlayerName = "Sally" };
            playerDoc.AddColor("blonde");
            playerDoc.AddColor("bronze");
            teamDoc.AddPlayer(playerDoc);

            TeamsCollection.InsertOne(teamDoc);

            teamDoc = new Team() { TeamName = "GoldDiggers", TeamCode = 1148 };

            playerDoc = new Player() { PlayerName = "Gary" };
            playerDoc.AddColor("green");
            playerDoc.AddColor("grey");
            teamDoc.AddPlayer(playerDoc);

            playerDoc = new Player() { PlayerName = "Greg" };
            playerDoc.AddColor("gold");
            playerDoc.AddColor("ganja");
            teamDoc.AddPlayer(playerDoc);

            playerDoc = new Player() { PlayerName = "George" };
            playerDoc.AddColor("gothBlack");
            playerDoc.AddColor("gothamGreen");
            teamDoc.AddPlayer(playerDoc);

            TeamsCollection.InsertOne(teamDoc);

            return TeamsCollection;
        }

        private class Team
        {
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

            public string TeamName { get; set; }

            public int TeamCode { get; set; }

            public List<Player> Players { get; set; } = new List<Player>();

            public void AddPlayer(Player player)
            {
                Players.Add(player);
            }
        }

        private class Player
        {
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

            public string PlayerName { get; set; }

            public List<string> PlayerColors { get; set; } = new List<string>();

            public void AddColor(string color)
            {
                PlayerColors.Add(color);
            }
        }
    }
}
