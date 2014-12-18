using System.Web.Http;

namespace AWStruck.Controllers.Api
{
  public class EnvController : ApiController
  {
    [Route("api/env/start")]
    [HttpGet]
    public string StartInstance()
    {
      return "Start";
    }

    [Route("api/env/stop")]
    [HttpGet]
    public string StopInstance()
    {
      return "Stop";
    }
  }
}
