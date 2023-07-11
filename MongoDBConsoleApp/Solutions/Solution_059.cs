using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{

    /// <summary>
    /// <a href="https://stackoverflow.com/questions/75074400/mongodb-how-do-i-join-the-second-collection-to-a-child-document-using-linq/75078153#75078153">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_059 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            var db = _client.GetDatabase("demo");

            var accountCollection = db.GetCollection<Account>("accounts");
            var userCollection = db.GetCollection<User>("users");

            var lookupPipeline = new EmptyPipelineDefinition<Account>()
                .Match(new BsonDocument("$expr",
                    new BsonDocument("$in", new BsonArray { "$accountId", "$$accountId" })
                ))
                .Project<Account, Account, ExtendedUserAccount>(
                    Builders<Account>.Projection
                        .Include(x => x.AccountId)
                        .Include(x => x.AccountCode));

            var extendedUser = userCollection.Aggregate()
                .Match(u => u.EmailAddress == "foo@bar.com")
                .Lookup(accountCollection,
                    new BsonDocument { { "accountId", "$userAccounts.accountId" } },
                    lookupPipeline,
                    new ExpressionFieldDefinition<ExtendedUser, IEnumerable<ExtendedUserAccount>>(x => x.UserAccounts)
                )
                .FirstOrDefault();

            Console.WriteLine(JsonConvert.SerializeObject(extendedUser, Formatting.Indented));
        }

        public Task RunAsync(IMongoClient _client)
        {
            return Task.Run(() => Run(_client));
        }

        //public record class User(string UserId, string EmailAddress, IEnumerable<UserAccount> UserAccounts);

        //public record class UserAccount(string AccountId);

        //public record class Account(string AccountId, string AccountCode, IEnumerable<string> UserIds);

        //public record class ExtendedUser(string UserId, string EmailAddress, IEnumerable<ExtendedUserAccount> UserAccounts);

        //public record class ExtendedUserAccount(string AccountId, string AccountCode);

        [BsonNoId]
        [BsonIgnoreExtraElements]
        public class User
        {
            [BsonConstructor]
            public User(string UserId, string EmailAddress, IEnumerable<UserAccount> UserAccounts)
            {
                this.UserId = UserId;
                this.EmailAddress = EmailAddress;
                this.UserAccounts = UserAccounts;
            }

            public string UserId { get; set; }
            public string EmailAddress { get; set; }
            public IEnumerable<UserAccount> UserAccounts { get; set; }
        }

        public class UserAccount
        {
            [BsonConstructor]
            public UserAccount(string AccountId)
            {
                this.AccountId = AccountId;
            }

            public string AccountId { get; set; }
        }

        [BsonNoId]
        [BsonIgnoreExtraElements]
        public class Account
        {
            [BsonConstructor]
            public Account(string AccountId, string AccountCode, IEnumerable<string> UserIds)
            {
                this.AccountId = AccountId;
                this.AccountCode = AccountCode;
                this.UserIds = UserIds;
            }

            public string AccountId { get; set; }
            public string AccountCode { get; set; }
            public IEnumerable<string> UserIds { get; set; }
        }

        [BsonIgnoreExtraElements]
        public class ExtendedUser
        {
            [BsonConstructor]
            public ExtendedUser(string UserId, string EmailAddress, IEnumerable<ExtendedUserAccount> UserAccounts)
            {
                this.UserId = UserId;
                this.EmailAddress = EmailAddress;
                this.UserAccounts = UserAccounts;
            }

            public string UserId { get; set; }
            public string EmailAddress { get; set; }
            public IEnumerable<ExtendedUserAccount> UserAccounts { get; set; }
        }

        [BsonIgnoreExtraElements]
        public class ExtendedUserAccount
        {
            [BsonConstructor]
            public ExtendedUserAccount(string AccountId, string AccountCode)
            {
                this.AccountId = AccountId;
                this.AccountCode = AccountCode;
            }

            public string AccountId { get; set; }
            public string AccountCode { get; set; }
        }
    }
}
