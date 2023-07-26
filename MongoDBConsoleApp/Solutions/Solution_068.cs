using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/75859023/how-to-filter-using-fhir-identifiers-from-c-sharp-to-mongodb/75866267#75866267">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_068 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<BsonDocument> _col = _db.GetCollection<BsonDocument>("FHIR");
            var builder = Builders<BsonDocument>.Filter;

            var filter = builder.ElemMatch("SerializedResource.identifier",
                builder.Eq("use", "official")
                & builder.Regex("value", $"/^value2$/i"));

            Helpers.PrintFormattedJson(_col.QueryToBson(filter));

            List<BsonDocument> result = (await _col.FindAsync(filter))
                .ToList();

            Helpers.PrintFormattedJson(result);
        }
    }
}
