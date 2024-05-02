using MongoDB.Driver;

namespace MongoEFCoreConsoleApp
{
    interface ISolution
    {
        void Run(IMongoClient _client);
        Task RunAsync(IMongoClient _client);
    }
}
