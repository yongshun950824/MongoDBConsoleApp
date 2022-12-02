﻿using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MongoDBConsoleApp.Program;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/74294539/in-mongodb-c-sharp-how-to-get-nested-array-to-perform-aggregation-query-on-it/74341177#74341177">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_050 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Tournament> _tournamentCollection = _db.GetCollection<Tournament>("tournament");

            var players = _tournamentCollection.Aggregate()
                .Unwind<Tournament, UnwindTournament>(tour => tour.Matches)
                .Group(m => m.Matches.WinnerId, g => new
                {
                    Id = g.Key,
                    MatchesWon = g.Count()
                })
                .ToList();

            PrintOutput(players);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        private void PrintOutput(dynamic result)
        {
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }

    public class Tournament
    {
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
        public string? Surface { get; set; }
        public int? DrawSize { get; set; }
        public string? Level { get; set; }
        public string? Date { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();
    }

    public class UnwindTournament
    {
        public ObjectId? Id { get; set; }
        public string? Name { get; set; }
        public string? Surface { get; set; }
        public int? DrawSize { get; set; }
        public string? Level { get; set; }
        public string? Date { get; set; }
        public Match Matches { get; set; } = new Match();
    }

    public class Match
    {
        public string Id { get; set; }
        public string? MatchNum { get; set; }
        public string? WinnerId { get; set; }
        public string? LoserId { get; set; }
        public string? Score { get; set; }
        public string? BestOf { get; set; }
        public string? Round { get; set; }
        public string? TourneyId { get; set; }
    }
}
