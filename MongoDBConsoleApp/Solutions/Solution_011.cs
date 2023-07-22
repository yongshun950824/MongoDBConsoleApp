using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MongoDBConsoleApp.Solutions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/questions/70742881/rename-nested-field-in-bsondocument-with-mongodb-driver/70749460#70749460">
    /// Question.
    /// </a>
    /// </summary>
    class Solution_011 : ISolution
    {
        public void Run(IMongoClient _client)
        {
            IMongoDatabase _database = _client.GetDatabase("demo");
            var collection = _database.GetCollection<Visit>("visit");

            var filter = Builders<Visit>.Filter;

            #region Solution 1
            //UpdateDefinition<Visit> update = GetUpdateDefinition1();
            #endregion

            #region Solution 2
            UpdateDefinition<Visit> update = GetUpdateDefinition2();
            #endregion
            collection.UpdateMany(filter.Empty, update);
        }

        public async Task RunAsync(IMongoClient _client)
        {
            await Task.Run(() => Run(_client));
        }

        /// <summary>
        /// Solution 1
        /// </summary>
        /// <returns></returns>
        private UpdateDefinition<Visit> GetUpdateDefinition1()
        {
            return Builders<Visit>.Update.Rename(x => x.CustomFields.Checkpoint, "CustomFields.Сheckpoint Comment-test");
        }

        /// <summary>
        /// Solution 2
        /// </summary>
        /// <returns></returns>
        private UpdateDefinition<Visit> GetUpdateDefinition2()
        {
            return Builders<Visit>.Update.Rename("CustomFields.Сheckpoint Comment", "CustomFields.Сheckpoint Comment-test");
        }

        class Visit
        {
            public ObjectId Id { get; set; }
            public CustomField CustomFields { get; set; }
        }

        class CustomField
        {
            [BsonElement("Сheckpoint Comment")]
            public BsonProp Checkpoint { get; set; }
            [BsonElement("Time of arrival at the checkpoint")]
            public BsonProp Time { get; set; }
        }

        class BsonProp
        {
            public ObjectId FieldId { get; set; }
            public string Type { get; set; }
            public string ValueBson { get; set; }
        }
    }
}
