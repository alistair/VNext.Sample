using System.Web.Http;
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
