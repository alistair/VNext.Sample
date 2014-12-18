using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AWStruck.AWS;
using AWStruck.Services;

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
     return Environments.GetEnvironments(Global.CreateAmazonClient());
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
