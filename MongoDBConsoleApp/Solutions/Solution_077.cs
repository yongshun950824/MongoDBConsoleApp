using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/77609329/delete-and-return-document-in-nested-array-with-mongodb-c-sharp-driver/77609926#77609926">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_077 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            string owner = "656dd33e0300";
            string cardId = "s39CNzu4Na3";

            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Holder> collection = _db.GetCollection<Holder>("holder");

            try
            {
                var result = await DeleteCardFromHolder(collection, owner, cardId);

                Helpers.PrintFormattedJson(result);
            }
            catch
            {
                throw;
            }
        }

        public async Task<dynamic> DeleteCardFromHolder(IMongoCollection<Holder> collection,
            string owner,
            string cardId)
        {
            var filter = Builders<Card>.Filter.Eq(x => x.Id, cardId);

            var holder = await collection.Find(
                    Builders<Holder>.Filter.Eq(e => e.Owner, owner) &
                    Builders<Holder>.Filter.ElemMatch(e => e.Cards, filter))
                .Project<Holder>(Builders<Holder>.Projection
                    .Include(x => x.Owner)
                    .ElemMatch(x => x.Cards, filter))
                .FirstOrDefaultAsync();

            if (holder == null || !holder.Cards.Any())
            {
                return new
                {
                    IsCardDeleted = false,
                    DeletedCards = (List<Card>)null
                };
            }

            UpdateResult res = await collection.UpdateOneAsync(
                Builders<Holder>.Filter.Eq(e => e.Owner, owner) &
                Builders<Holder>.Filter.ElemMatch(e => e.Cards, filter),
                Builders<Holder>.Update.PullFilter(e => e.Cards, filter)
            );

            // Update and get document before update
            //var before = await collection.FindOneAndUpdateAsync(
            //    Builders<Holder>.Filter.Eq(e => e.Owner, owner) &
            //    Builders<Holder>.Filter.ElemMatch(e => e.Cards, filter),
            //    Builders<Holder>.Update.PullFilter(e => e.Cards, filter),
            //    new FindOneAndUpdateOptions<Holder, Holder>() { ReturnDocument = ReturnDocument.Before });

            if (res.ModifiedCount == 0)
            {
                return new
                {
                    IsCardDeleted = false,
                    DeletedCards = (List<Card>)null
                };
            }

            return new
            {
                IsCardDeleted = true,
                DeletedCards = holder.Cards
            };
        }

        internal class Holder
        {
            public ObjectId Id { get; set; }
            public string Owner { get; set; }
            public List<Card> Cards { get; set; }
        }

        [BsonNoId]
        internal class Card
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Placement { get; set; }
        }
    }
}
