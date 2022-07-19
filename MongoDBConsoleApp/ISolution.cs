using MongoDB.Driver;
using System.Threading.Tasks;

namespace MongoDBConsoleApp
{
    interface ISolution
    {
        void Run(IMongoClient _client);
        Task RunAsync(IMongoClient _client);
    }
}
