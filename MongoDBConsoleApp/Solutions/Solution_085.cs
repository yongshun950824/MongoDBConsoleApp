using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/78873427/how-to-filter-on-an-array-of-objects-that-equals-name-and-contains-value/78874082#78874082">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_085 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<RootModel> _collection = _db.GetCollection<RootModel>("demo");

            #region Solution 1
            //var results = _collection.AsQueryable().Where(item =>
            //    item.Info.Any(info =>
            //        info.Name == "John" &&
            //        info.Random.Any(random => random == "31")));
            #endregion Solution 1

            #region Solution 2
            var filter = Builders<RootModel>.Filter.ElemMatch(x => x.Info,
                Builders<InfoModel>.Filter.Eq(y => y.Name, "John")
                & Builders<InfoModel>.Filter.AnyIn(y => y.Random, new string[] { "31" }));

            var results = await _collection.Find(filter)
                .ToListAsync();
            #endregion Solution 2

            Helpers.PrintFormattedJson(results);
        }
    }

    class RootModel
    {
        [BsonId]
        public int Id { get; set; }

        public InfoModel[] Info { get; set; }
    }

    [BsonNoId]
    class InfoModel
    {
        public string Name { get; set; }

        public string[] Random { get; set; }
    }
}
