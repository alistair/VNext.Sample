using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using AWStruck.AWS;
using AWStruck.Hubs;
using AWStruck.Models;
using AWStruck.Mongo;
using AWStruck.Savings;

using Hangfire;

using Microsoft.AspNet.SignalR;

using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace AWStruck.Controllers.Api
{
    public class EnvController : ApiController
    {
    protected readonly Lazy<IHubContext> SwitchHub =
      new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<SwitchHub>());


        [HttpGet]
        public IEnumerable<Environment> Index()
        {
            return Environments.GetEnvironments(Global.CreateAmazonClient()).Select(Map);
        }

        private Environment Map(Environment env)
        {
            List<BsonDocument> results = MongoProvider.Database.Value.GetCollection("hangfire.hash")
                .AsQueryable()
                .Where(x => ((string)x["Key"]).StartsWith(string.Format("recurring-job:{0}", env.Name)) && x["Field"] == "Cron")
                .ToList();

            CronDescription[] desc = results.Select(
                x => new CronDescription
                {
                    Name = x["Key"].AsString.Split('_')[1],
        Description = Cron.GetDescription((string)x["Value"])
                }).ToArray();

            return env.CloneWithAutoAndDescriptions(results.Any(), desc);
        }

        [HttpGet]
    [Route("api/env/nogo/{envId}")]
    public string StopEnv([FromUri] string envId)
        {
            Environments.StopEnvironmentInternal(envId);

      SwitchHub.Value.Clients.All.signal(
        new InstanceStatus
        {
          Id = envId,
          Status = "Stopped"
        });

            return "done";
        }

        [HttpGet]
    [Route("api/env/go/{envId}")]
    public string StartEnv([FromUri] string envId)
        {
            Environments.StartEnvironmentInternal(envId);

      SwitchHub.Value.Clients.All.signal(
        new InstanceStatus
        {
          Id = envId,
          Status = "Started"
        });
            return "done";
        }

        [HttpGet]
    [Route("api/env/createSchedule/{envId}")]
        public string CreateEnvironment([FromUri] string envId)
        {
      RecurringJob.AddOrUpdate(string.Format("{0}_start", envId), Environments.StartEnvironment(envId), "5 * * * *");
      RecurringJob.AddOrUpdate(string.Format("{0}_stop", envId), Environments.StopEnvironment(envId), "5 * * * *");
            return "done";
        //Deprecated
        //Deprecated
    }

    [Route("api/savings")]
    [HttpGet]
    public SavingsResponse GetSavings()
        {
            var savingsHelper = new SavingsHelper();
            return savingsHelper.CalculateSavings();
        }
    }
}
