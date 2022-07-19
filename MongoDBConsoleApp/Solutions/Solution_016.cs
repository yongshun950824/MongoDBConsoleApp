using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70839664/how-to-write-this-mongodb-aggregate-query-into-c-sharp/70842890#70842890">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_016 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");

            var result = GetResultWithBsonDocument(_db);

            PrintOutput(result);
        }

        public Task RunAsync(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <returns></returns>
        private dynamic GetResultWithFullAggregateFluent(IMongoDatabase _db)
        {
            var result = _db.GetCollection<Employee>("Employee")
               .Aggregate()
               .Unwind<Employee, UnwindEmployeeProject>(i => i.Projects)
               .Group(
                   k => k.EmpId,
                   g => new
                   {
                       EmpId = g.Key,
                       LastUpdated = g.Max(x => x.Projects.LastUpdated)
                   })
               .ToList();

            return result;
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <returns></returns>
        private List<BsonDocument> GetResultWithMixAggregateFluent(IMongoDatabase _db)
        {
            return _db.GetCollection<Employee>("Employee")
                .Aggregate()
                .Unwind(i => i.Projects)
                .Group(new BsonDocument
                {
                    { "_id", "$EmpId" },
                    { "LastUpdated", new BsonDocument("$max", "$Projects.LastUpdated") }
                })
                .ToList();
        }

        /// <summary>
        /// Solution 3
        /// </summary>
        /// <returns></returns>
        private List<BsonDocument> GetResultWithBsonDocument(IMongoDatabase _db)
        {
            PipelineDefinition<Employee, BsonDocument> pipeline = new BsonDocument[]
            {
                new BsonDocument("$unwind", "$Projects"),
                new BsonDocument("$group",
                    new BsonDocument
                    {
                        { "_id", "$EmpId" },
                        { "LastUpdated",
                            new BsonDocument("$max", "$Projects.LastUpdated") }
                    })
            };

            return _db.GetCollection<Employee>("Employee")
                .Aggregate(pipeline)
                .ToList();
        }

        private void PrintOutput(dynamic result)
        {
            Console.WriteLine(result.ToJson());
        }

        class Employee
        {
            public int EmpId { get; set; }
            public List<EmployeeProject> Projects { get; set; }
        }

        class EmployeeProject
        {
            public DateTime LastUpdated { get; set; }
        }

        class UnwindEmployeeProject
        {
            public int EmpId { get; set; }
            public EmployeeProject Projects { get; set; }
        }
    }
}
