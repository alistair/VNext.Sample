using System;
using System.Web.Http;

using AWStruck.Hubs;
using AWStruck.Models;
using AWStruck.Services;

using Microsoft.AspNet.SignalR;

namespace AWStruck.Controllers.Api
{
    public class EnvController : ApiController
    {
        private readonly EnvService _envService;
        protected readonly Lazy<IHubContext> SwitchHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<SwitchHub>());

        public EnvController()
        {
            //_envService = new EnvService();
        }

        [Route("api/env/start")]
        [HttpGet]
        public void StartInstance()
        {
            //_envService.Start();

            var v = SwitchHub.Value.Clients.All.signal(new InstanceStatus(){Instance = "blah", Status = "Started"});
        }

        [Route("api/env/stop")]
        [HttpGet]
        public void StopInstance()
        {
            //_envService.Stop();

            SwitchHub.Value.Clients.All.signal(new InstanceStatus() { Instance = "blah", Status = "Stopped" });
        }
    }
}
