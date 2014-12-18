using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AWStruck.AWS;
using AWStruck.Mongo;
using AWStruck.Services;
using Hangfire;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AWStruck.Controllers.Api
{
  public class EnvController : ApiController
  {
    private EnvService _envService;

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
    [Route("api/env/nogo")]
    public string StopEnv([FromUri]string envId)
    {
      Environments.StopEnvironmentInternal(envId);
      return "done";
    }

    [HttpGet]
    [Route("api/env/go")]
    public string StartEnv(string envId)
    {
      Environments.StartEnvironmentInternal(envId);
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

		[Route("api/env/start")]
    [HttpGet]
    public void StartInstance()
    {
      _envService.Start();
    }

    [Route("api/env/stop")]
    [HttpGet]
    public void StopInstance()
    {
      _envService.Stop();
    }
  }
}
