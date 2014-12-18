using System;
using System.Configuration;
using Antlr.Runtime.Misc;
using Hangfire.Mongo;
using MongoDB.Driver;

namespace AWStruck.Mongo
{
  public static class MongoProvider
  {
    public static Lazy<MongoDatabase> Database = new Lazy<MongoDatabase>(() =>
    {
      var connectionString = ConfigurationManager.AppSettings["MongoDB.ConnectionString"];
      var database = ConfigurationManager.AppSettings["MongoDB.Database"];
      return GetDatabase(GetMongoClient(connectionString), database);
    }); 

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
