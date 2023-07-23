using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/71505601/mongodb-how-to-push-an-embedded-array-into-an-existing-document/71506079#71506079">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_020 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            var _userPortfoliosCollection = _db.GetCollection<UserPortfolioList>("UserPortfolios");

            var userPflist = new UserPortfolioList
            {
                Username = "aaa",
                Pflist = new List<Pflist>
                {
                    new Pflist
                    {
                        PfName = "US TECH C",
                        Symbols = new List<string>
                        {
                            "AAPL",
                            "TSLA"
                        }
                    }
                }
            };

            var result = await UpdateWithPushSingleElement(_userPortfoliosCollection, userPflist);

            Helpers.PrintFormattedJson(result);
        }

        private Task<UpdateResult> UpdateWithPushSingleElement(
            IMongoCollection<UserPortfolioList> _userPortfoliosCollection,
            UserPortfolioList userPflist)
        {
            return _userPortfoliosCollection.UpdateOneAsync(
                Builders<UserPortfolioList>.Filter.Eq("Username", userPflist.Username),
                Builders<UserPortfolioList>.Update.Push(x => x.Pflist, userPflist.Pflist[0]));
        }

        private Task<UpdateResult> UpdateWithPushEachElement(
            IMongoCollection<UserPortfolioList> _userPortfoliosCollection,
            UserPortfolioList userPflist)
        {
            return _userPortfoliosCollection.UpdateOneAsync(
                Builders<UserPortfolioList>.Filter.Eq("Username", userPflist.Username),
                Builders<UserPortfolioList>.Update.PushEach(x => x.Pflist, userPflist.Pflist));
        }

        [BsonIgnoreExtraElements]
        class UserPortfolioList
        {
            [BsonElement("username")]
            public string Username { get; set; }
            [BsonElement("pflist")]
            public List<Pflist> Pflist { get; set; }
        }

        class Pflist
        {
            [BsonElement("pfName")]
            public string PfName { get; set; } = "DEFAULT NAME";
            [BsonElement("symbols")]
            public List<string> Symbols { get; set; }
        }
    }
}
