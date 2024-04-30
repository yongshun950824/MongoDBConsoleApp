// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoEFCoreConsoleApp;
using MongoEFCoreConsoleApp.Solutions;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

var _client = new MongoClient(config.GetConnectionString("MongoUri"));

ISolution solution = new Solution_081();
await solution.RunAsync(_client);

Console.ReadLine();
