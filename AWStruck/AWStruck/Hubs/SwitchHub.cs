using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AWStruck.AWS;
using AWStruck.Models;
using AWStruck.Mongo;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using MongoDB.Driver.Linq;

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

        public override Task OnConnected()
        {
            IEnumerable<Environment> environments = Environments.GetEnvironments(Global.CreateAmazonClient()).Select(x => x.CloneWithAuto(IsAuto(x.Name)));
            Clients.All.signal(environments);

            return base.OnConnected();
        }

        private bool IsAuto(string envId)
        {
            return MongoProvider.Database.Value.GetCollection("hangfire.set")
                .AsQueryable()
                .Count(x => ((string)x["Key"]).StartsWith(string.Format("recurring-job:{0}", envId))) > 0;
        }
    }
}
