using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using AWStruck.AWS;
using AWStruck.Models;

using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AWStruck.Hubs
{
    [HubName("switchHub")]
    public class SwitchHub : Hub
    {
        [HubMethodName("stopEnv")]
        public void StopEnv(string envId)
        {
            Environments.StopEnvironmentInternal(envId);

            Clients.All.signal(
                new InstanceStatus
                {
                    Id = envId,
                    Status = "Stopped"
                });
        }

        [HubMethodName("startEnv")]
        public void StartEnv(string envId)
        {
            Environments.StartEnvironmentInternal(envId);

            Clients.All.signal(
                new InstanceStatus
                {
                    Id = envId,
                    Status = "Started"
                });
        }
    }
}
