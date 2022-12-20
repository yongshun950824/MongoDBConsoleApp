using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using static MongoDBConsoleApp.Program;

namespace MongoDBConsoleApp.Solutions
{
    internal class Solution_052 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            this.RunAsync(_client).GetAwaiter().GetResult();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            Helpers.RegisterCamelCasePack();

            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<TransactionModel> transactionCollection = _db.GetCollection<TransactionModel>("transactions");

            DateTime fromDate = new DateTime(2022, 11, 1);
            DateTime toDate = new DateTime(2022, 11, 30);

            FilterDefinitionBuilder<TransactionModel> filterBuilder = new FilterDefinitionBuilder<TransactionModel>();

            var monthlyNonIncomefilter = filterBuilder.Gte(x => x.Date, fromDate) &
                filterBuilder.Lte(x => x.Date, toDate);

            var monthlyTransactions = await transactionCollection.Aggregate()
                .Match(monthlyNonIncomefilter)
                .Group(
                    x => x.Category == "Income" ? "Income" : "MonthlySpent",
                    group => new
                    {
                        Type = group.Key,
                        Price = group.Sum(x => x.Price)
                    })
                .ToListAsync();

            Console.WriteLine(JsonConvert.SerializeObject(monthlyTransactions, Formatting.Indented));
        }

        public class TransactionModel
        {
            public ObjectId Id { get; set; }
            public double Price { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
