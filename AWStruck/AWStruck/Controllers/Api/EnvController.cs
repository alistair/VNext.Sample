using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Http;

namespace AWStruck.Controllers.Api
{
  public class EnvController
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
