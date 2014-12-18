using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Web.Http;

using Amazon.EC2.Model;

using AWStruck.AWS;
using AWStruck.Mongo;
using AWStruck.Hubs;
using AWStruck.Services;
using Hangfire;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using AWStruck.Savings;

using Microsoft.AspNet.SignalR;

using InstanceStatus = AWStruck.Models.InstanceStatus;

namespace AWStruck.Controllers.Api
{
  public class EnvController : ApiController
  {
    protected readonly Lazy<IHubContext> SwitchHub =
      new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<SwitchHub>());

    private readonly EnvService _envService;

    public EnvController()
    {
      _envService = new EnvService();
    }

    [HttpGet]
    public IEnumerable<Environment> Index()
    {
      return Environments.GetEnvironments(Global.CreateAmazonClient()).Select(Map);
    }

    private Environment Map(Environment env)
    {
      var results = MongoProvider.Database.Value.GetCollection("hangfire.hash")
        .AsQueryable()
        .Where(x => ((string) x["Key"]).StartsWith(string.Format("recurring-job:{0}", env.Name)) && x["Field"] == "Cron")
        .ToList();

      var desc = results.Select(x => new CronDescription
      {
        Name = x["Key"].AsString.Split('_')[1],
        Description = Cron.GetDescription((string) x["Value"])
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
      RecurringJob.AddOrUpdate(string.Format("{0}_start", envId), Environments.StartEnvironment(envId), "5 6 * * *");
      RecurringJob.AddOrUpdate(string.Format("{0}_stop", envId), Environments.StopEnvironment(envId), "5 20 * * *");
      return "done";
    }

    //Deprecated
    [Route("api/env/start/")]
    [HttpGet]
    public StartInstancesResponse StartInstance()
    {
      return _envService.Start();
    }

    //Deprecated
    [Route("api/env/stop")]
    [HttpGet]
    public StopInstancesResponse StopInstance()
    {
      return _envService.Stop();
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
