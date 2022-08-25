using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    internal class Solution_023 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            var result = await Get<User>("621f9073e27aaf55a5b9f9ac");

            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        private static async Task<TEntity> Get<TEntity>(string id) where TEntity : IEntity
        {
            var mongoUri = ConfigurationManager.ConnectionStrings["MongoUri"].ToString();

            MongoClient _client = new MongoClient(mongoUri);
            IMongoDatabase _database = _client.GetDatabase("demo");

            var dbCollection = _database.GetCollection<TEntity>(typeof(TEntity).GetCollectionName());
            var asyncCursor = await dbCollection.FindAsync(x => x.Id == id);
            return await asyncCursor.FirstOrDefaultAsync();
        }

        [BsonIgnoreExtraElements]
        class User : IEntity
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            //[RepositoryID]
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string CountryCode { get; set; }
            public string Phone { get; set; }
            public string Extension { get; set; }
            public IEnumerable<UserRole> UserRoles { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime ModifiedDate { get; set; }
        }

        class UserRole
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; }
        }

        interface IEntity
        {
            string Id { get; set; }
        }
    }
}

namespace MongoDBConsoleApp
{
    public static class TypeExtensions
    {
        public static string GetCollectionName(this Type type)
        {
            return type.Name;
        }
    }
}
