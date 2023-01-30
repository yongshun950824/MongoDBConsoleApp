using MongoDB.Bson.Serialization.Conventions;

namespace MongoDBConsoleApp
{
    public static class Helpers
    {
        public static void RegisterCamelCasePack()
        {
            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("camel case", pack, t => true);
        }
    }
}
