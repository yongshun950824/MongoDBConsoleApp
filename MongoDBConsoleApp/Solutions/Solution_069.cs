using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/76740049/mongodb-how-to-query-with-multiple-and-conditions-for-phone-numbers-using-filt/76742141#76742141">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_069 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            this.RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Applicant> _col = _db.GetCollection<Applicant>("people");

            #region Solution 1
            var queryFilter = GetQueryFilterWithAndOperator();
            #endregion

            #region Solution 2
            //var queryFilter = GetQueryFilterWithAllOperator();
            #endregion

            var cursor = await _col.FindAsync(queryFilter);
            List<Applicant> result = await cursor.ToListAsync();

            PrintOutput(result);
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <returns></returns>
        private FilterDefinition<Applicant> GetQueryFilterWithAndOperator()
        {
            var filterBuilder = Builders<Applicant>.Filter;

            var homePhoneFilter = filterBuilder.ElemMatch(
                x => x.PhoneNumbers,
                y => y.Type == "home" && y.Value == "111-111-1111");
                
            var cellPhoneFilter = filterBuilder.ElemMatch(
                x => x.PhoneNumbers,
                y => y.Type == "cell" && y.Value == "222-222-2222"); 

            return homePhoneFilter & cellPhoneFilter;
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <returns></returns>
        private FilterDefinition<Applicant> GetQueryFilterWithAllOperator()
        {
            var filterBuilder = Builders<Applicant>.Filter;
            return filterBuilder.All(x => x.PhoneNumbers, new List<PhoneNumber>
            {
                new PhoneNumber { Type = "home", Value = "111-111-1111" },
                new PhoneNumber { Type = "cell", Value = "222-222-2222" }
            });
        }

        private void PrintOutput(List<Applicant> result)
        {
            Console.WriteLine(result.ToJson(new JsonWriterSettings
            {
                Indent = true
            }));
        }

        public class Applicant
        {
            public ObjectId Id { get; set; }
            public string LastName { get; set; }
            public string Type { get; set; }
            public PhoneNumber[] PhoneNumbers { get; set; }
        }

        public class PhoneNumber
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
    }
}
