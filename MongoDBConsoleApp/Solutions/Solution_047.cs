using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/74131498/mongodb-net-driver-how-to-search-the-documents-with-fulfilling-in-the-neste/74133584#74133584">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_047 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Book> _mongoCollection = _db.GetCollection<Book>("book");

            FilterDefinition<Book> filter = Builders<Book>.Filter.Empty;

            filter &= Builders<Book>.Filter.ElemMatch(x => x.Editor,
                Builders<Editor>.Filter.And(
                    Builders<Editor>.Filter.Eq(y => y.FirstName, "Jane"),
                    Builders<Editor>.Filter.Eq(y => y.LastName, "Smith")
                )
            );

            filter &= Builders<Book>.Filter.ElemMatch(x => x.Editor,
                Builders<Editor>.Filter.And(
                    Builders<Editor>.Filter.Eq(y => y.FirstName, "Jennifer"),
                    Builders<Editor>.Filter.Eq(y => y.LastName, "Lopez")
                )
            );

            List<Book> books = (await _mongoCollection.FindAsync(filter))
                .ToList();

            Helpers.PrintFormattedJson(books);
        }

        class Book
        {
            [BsonId]
            public ObjectId Id { get; set; }

            public string Isbn { get; set; }

            public Author Author { get; set; }

            public Editor[] Editor { get; set; }

            public string Title { get; set; }

            public string[] Category { get; set; }
        }

        class Author : People
        {

        }

        class Editor : People
        {

        }

        abstract class People
        {
            [BsonElement("_id")]
            public int Id { get; set; }

            public string LastName { get; set; }

            public string FirstName { get; set; }
        }
    }
}
