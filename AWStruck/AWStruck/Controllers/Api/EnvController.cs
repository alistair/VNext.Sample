using System;
using System.Collections.Generic;
using System.Web.Http;

using Amazon.EC2.Model;

using AWStruck.AWS;
using AWStruck.Hubs;
using AWStruck.Services;

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
            return Environments.GetEnvironments(Global.CreateAmazonClient());
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
    }
}
