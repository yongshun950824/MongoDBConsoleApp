using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    internal class Solution_058 : ISolution
    {
        /// <summary>
        /// <a href="https://stackoverflow.com/questions/75067318/mongodb-c-net-driver-how-to-deserialize-uuid-and-isodate/75071541#75071541">
        /// Question.
        /// </a>
        /// </summary>
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<ReportRequest> _collection = _db.GetCollection<ReportRequest>("report_request");

            string companyId = "XYZ";

            var filter = Builders<ReportRequest>.Filter.Eq(r => r.CompanyId, companyId);
            var cursor = await _collection.FindAsync(filter);
            var reportRequests = await cursor.ToListAsync();

            foreach (var req in reportRequests)
                Console.WriteLine($"Id: {req.Id.ToString()}, Date: {req.RequestDate}");
        }

        #region Solution 1
        //class ReportRequest
        //{
        //    public ReportRequest(string companyId)
        //    {
        //        this.Id = Guid.NewGuid();
        //        this.CompanyId = companyId;
        //        this.RequestDate = DateTime.UtcNow;
        //    }

        //    [BsonId]
        //    [BsonGuidRepresentation(GuidRepresentation.Standard)]
        //    public Guid Id { get; set; }

        //    public string CompanyId { get; set; }

        //    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        //    public DateTime RequestDate { get; set; }
        //}
        #endregion

        #region Solution 2
        class ReportRequest
        {
            public ReportRequest(string companyId)
            {
                this.Id = Guid.NewGuid();
                this.CompanyId = companyId;
                this.RequestDate = DateTime.UtcNow;
            }

            public ReportRequest(Guid id, string companyId, DateTime requestDate)
            {
                this.Id = id;
                this.CompanyId = companyId;
                this.RequestDate = requestDate;
            }

            [BsonId]
            [BsonGuidRepresentation(GuidRepresentation.Standard)]
            public Guid Id { get; }

            public string CompanyId { get; }

            [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
            public DateTime RequestDate { get; }
        }
        #endregion
    }
}
