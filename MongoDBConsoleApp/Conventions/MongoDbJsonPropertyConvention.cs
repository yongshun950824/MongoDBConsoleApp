using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace MongoDBConsoleApp.Conventions
{
    /// <summary>
    /// <a href="https://stackoverflow.com/a/78276348/8017690">JsonPropertyAttribute PropertyName with MongoDB</a>
    /// </summary>
    public class MongoDbJsonPropertyConvention : ConventionBase, IMemberMapConvention
    {
        public void Apply(BsonMemberMap memberMap)
        {
            memberMap.SetElementName(GetElementName(memberMap) ?? memberMap.MemberName);
        }

        private string GetElementName(BsonMemberMap memberMap)
        {
            if (!String.Equals(memberMap.MemberName, "Id", StringComparison.OrdinalIgnoreCase))
            {
                var jsonPropertyAtt = memberMap.MemberInfo.GetCustomAttribute<JsonPropertyAttribute>();
                if (!string.IsNullOrEmpty(jsonPropertyAtt.PropertyName))
                {
                    return jsonPropertyAtt.PropertyName;
                }
            }
            return null;
        }
    }
}
