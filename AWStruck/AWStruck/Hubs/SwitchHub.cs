using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AWStruck.AWS;
using AWStruck.Models;
using AWStruck.Mongo;

using Hangfire;

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

        [HubMethodName("onConnected")]
        public override Task OnConnected()
        {
            IEnumerable<Environment> environments = Environments.GetEnvironments(Global.CreateAmazonClient()).Select(Map);
            Clients.All.signal(environments);

            return base.OnConnected();
        }


        private Environment Map(Environment env)
        {
            var results = MongoProvider.Database.Value.GetCollection("hangfire.hash")
              .AsQueryable()
              .Where(x => ((string)x["Key"]).StartsWith(string.Format("recurring-job:{0}", env.Name)) && x["Field"] == "Cron")
              .ToList();

            var desc = results.Select(x => new CronDescription
            {
                Name = x["Key"].AsString.Split('_')[1],
                Description = Cron.GetDescription((string)x["Value"])
            }).ToArray();

            return env.CloneWithAutoAndDescriptions(results.Any(), desc);
        }
    }
}
