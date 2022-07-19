using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    class Solution_029 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            IMongoCollection<Team> _collection = _database.GetCollection<Team>("team");

            FilterDefinition<Team> filter = Builders<Team>.Filter.Empty;

            var result = _collection.Find(filter).Project(x => new Team
            {
                Id = x.Id,
                Name = x.Name,
                //TeamData = new TeamData
                //{
                //    Players = x.TeamData.Players.Select(y => new Player
                //    {
                //        LastName = y.LastName
                //    }).ToList()
                //},
                TeamData = x.TeamData

            });

            PrintOutput(result.ToList());
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private void PrintOutput(List<Team> result)
        {
            Console.WriteLine(result.ToJson(new MongoDB.Bson.IO.JsonWriterSettings
            {
                Indent = true
            }));
        }

        class Team
        {
            [BsonId]
            public ObjectId Id { get; set; }

            public string Name { get; set; }

            public TeamData TeamData { get; set; }
        }

        class TeamData
        {
            public IEnumerable<Player> Players { get; set; }
        }

        class Player
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        class TeamDto
        {
            [BsonId]
            public ObjectId Id { get; set; }

            public string Name { get; set; }

            public TeamDataDto TeamData { get; set; }
        }

        class TeamDataDto
        {
            public IEnumerable<PlayerDto> Players { get; set; }
        }

        class PlayerDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
