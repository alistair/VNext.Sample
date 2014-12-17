using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AWStruck.Controllers.Api
{
  public class EnvController : ApiController
  {
    [GET("/api/env/start")]
    [HttpGet]
    public string StartInstance()
    {
      return "Start";
    }

    [GET("/api/env/stop")]
    [HttpGet]
    public string StopInstance()
    {
      return "Stop";
    }
  }
}
