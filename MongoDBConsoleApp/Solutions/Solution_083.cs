using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/78814121/mongodb-how-to-filter-and-update-on-a-child-of-a-child/78814123#78814123">
    /// Question.
    /// </a>
    /// </summary>
    internal class Solution_083 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<Account> _collection = _db.GetCollection<Account>("accounts");

            var headers = new
            {
                AccountID = ObjectId.Parse("66a9879d1f9279787a7e7f16"),
                StoreID = ObjectId.Parse("66a9879d1f9279787a7e7f17"),
            };

            var customer = new Customer
            {
                UUID = Guid.Parse("2e29f91d-c3eb-46f5-869c-2cf152349eb3"),
                Name = "<New> name"
            };
            var customerUUID = customer.UUID;

            var filter = Builders<Account>.Filter.Eq(e => e.ID, headers.AccountID)
                & Builders<Account>.Filter.ElemMatch(e => e.Stores, Builders<Store>.Filter.Eq(e => e.ID, headers.StoreID))
                & Builders<Account>.Filter.ElemMatch(e => e.Stores,
                    Builders<Store>.Filter.ElemMatch(e => e.Customers, Builders<Customer>.Filter.Eq(e => e.UUID, customerUUID)));
            var update = Builders<Account>.Update.Set(e => e.Stores.FirstMatchingElement().Customers.AllMatchingElements("c"), customer);

            var result = await _collection.UpdateOneAsync(filter, update,
                new UpdateOptions
                {
                    ArrayFilters = new ArrayFilterDefinition[]
                    {
                        new BsonDocumentArrayFilterDefinition<Customer>
                        (
                            new BsonDocument("c.UUID", BsonValue.Create(customerUUID.ToString()))
                        )
                    }
                });

            Helpers.PrintFormattedJson(result);
        }

        [BsonIgnoreExtraElements]
        class Account
        {
            [BsonId]
            public ObjectId ID { get; set; }
            public string Name { get; set; }
            public string APIKey { get; set; }
            public string Secret { get; set; }
            public bool Active { get; set; }

            public List<Store> Stores { get; set; } = new List<Store>();

            public List<User> Users { get; set; } = new List<User>();
        }

        class User
        { }

        [BsonIgnoreExtraElements]
        class Store
        {
            [BsonId]
            public ObjectId ID { get; set; }

            public string Name { get; set; }
            public string Location { get; set; }
            public DateTime LastMessageDate { get; set; }
            public List<Customer> Customers = new List<Customer>();
        }

        [BsonIgnoreExtraElements]
        class Customer
        {
            [BsonRepresentation(BsonType.String)]
            public Guid UUID { get; set; }
            public string Name { get; set; }
        }
    }
}
