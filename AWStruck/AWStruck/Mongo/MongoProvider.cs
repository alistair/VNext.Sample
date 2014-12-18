using Antlr.Runtime.Misc;
using Hangfire.Mongo;
using MongoDB.Driver;

namespace AWStruck.Mongo
{
  public class MongoProvider
  {
    //public static Action<Action<MongoCollection<>>
    //public static MongoCollectio

    public static MongoDatabase GetDatabase(MongoClient client, string databaseName)
    {
      return client.GetServer().GetDatabase(databaseName);
    }

    public static MongoClient GetMongoClient(string connectionString)
    {
      return new MongoClient(connectionString);
    }
  }
}
