using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/73704002/mongodb-net-driver-how-to-push-an-element-into-an-array-which-is-inside-an-ar/73710622#73710622">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_043 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(IMongoClient _client)
        {
            IMongoDatabase _db = _client.GetDatabase("demo");
            IMongoCollection<MessageLog> _col = _db.GetCollection<MessageLog>("MessageLog");

            object responseMessageActionLog = new
            {
                LogDateTime = DateTime.UtcNow,
                Contents = "New Response Message Action Log"
            };

            FilterDefinition<MessageLog> filter = Builders<MessageLog>.Filter.Eq(e => e.MessageTrackingId, "08eab450-a2bd-408a-afab-be9ea665503e");
            filter &= Builders<MessageLog>.Filter.Eq("RequestMessageLog.BiDirectionalMessageLogs.MessageTrackingId", "bb7b5573-15ae-4db9-a67e-6862f31b9437");

            UpdateDefinition<MessageLog> update = Builders<MessageLog>.Update
                .Push("RequestMessageLog.BiDirectionalMessageLogs.$.RequestMessageLog.ResponseMessageLog.ResponseMessageActionLogs", responseMessageActionLog);

            UpdateResult result = await _col.UpdateOneAsync(filter, update);
            PrintOutput(result);
        }

        private void PrintOutput(UpdateResult result)
        {
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }

    class MessageLog
    {
        public string MessageTrackingId { get; set; }
    }
}
