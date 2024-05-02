using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace MongoEFCoreConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/78356762/mongodb-entity-framework-core-expression-must-be-writeable/78360100#78360100">
    /// Question 1.
    /// </a>
    /// <a href="https://stackoverflow.com/questions/78359482/entity-framework-save-private-field/78360277#78360277">
    /// Question 2.
    /// </a>
    /// </summary>
    internal class Solution_081 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            var db = MongoDataContext.Create(_client.GetDatabase("demo"));

            db.Accounts.Add(new Account("user001", "Abc1234"));
            db.SaveChanges();
        }

        #region DB Contexts
        internal class MongoDataContext : DbContext
        {
            public DbSet<Account> Accounts { get; init; }

            public static MongoDataContext Create(IMongoDatabase database) =>
                new(new DbContextOptionsBuilder<MongoDataContext>()
                    .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                    .Options);

            public MongoDataContext(DbContextOptions options)
                : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Account>()
                    .Property(x => x.Salt)
                    .HasField("salt");

                modelBuilder.Entity<Account>()
                    .ToCollection("accounts");
            }
        }
        #endregion

        #region Models
        internal class SignInCredentials(string username, string? password)
        {
            public string Username { get; set; } = username;

            /// <summary>
            /// <a href="https://stackoverflow.com/questions/78356762/mongodb-entity-framework-core-expression-must-be-writeable/78360100#78360100">Question</a>
            /// </summary>
            public string? Password { get; set; } = password;
        }

        internal class Account : SignInCredentials
        {
            // [Key] sets the primary key
            [Key]
            public ObjectId Id { get; private set; }

            private readonly byte[] salt;

            public byte[] Salt
            {
                get { return salt; }
            }

            public Account(string username, string password) : base(username, null)
            {
                Id = new ObjectId();
                salt = GenerateSalt();
                Password = Hash(password, salt);
            }

            // More code not displayed

            public byte[] GenerateSalt()
            {
                var salt = new byte[32];
                using (var random = RandomNumberGenerator.Create())
                {
                    random.GetNonZeroBytes(salt);
                }

                return salt;
            }

            public string Hash(string password, byte[] salt)
            {
                return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
            }
        }
        #endregion
    }
}
