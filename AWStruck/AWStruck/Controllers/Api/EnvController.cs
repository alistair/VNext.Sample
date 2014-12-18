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
        protected readonly Lazy<IHubContext> SwitchHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<SwitchHub>());
        private readonly EnvService _envService;

    public EnvController()
    {
      _envService = new EnvService();
    }

    [HttpGet]
    public IEnumerable<Environment> Index()
    {
      return Environments.GetEnvironments(Global.CreateAmazonClient()).Select(x => x.CloneWithAuto(IsAuto(x.Name)));
    }

    private bool IsAuto(string envId)
    {
      return MongoProvider.Database.Value.GetCollection("hangfire.set")
        .AsQueryable()
        .Count(x => ((string) x["Key"]).StartsWith(string.Format("recurring-job:{0}", envId))) > 0;
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
    [Route("api/env/createSchedule")]
    public string CreateEnvironment([FromUri] string envId)
    {
      RecurringJob.AddOrUpdate(string.Format("{0}_start", envId), Environments.StartEnvironment(envId), "0 6 * * 1,2,3,4,5");
      RecurringJob.AddOrUpdate(string.Format("{0}_stop", envId), Environments.StopEnvironment(envId), "0 19 * * 1,2,3,4,5");
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

        [Route("api/env")]
        [HttpGet]
        public IEnumerable<Environment> Envs()
        {
            return Environments.GetEnvironments(Global.CreateAmazonClient());
        }

        [Route("api/savings")]
        [HttpGet]
        public SavingsResponse GetSavings()
    {
            return SavingsHelper.CalculateSavings();
    }
  }
}
