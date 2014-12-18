using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using AWStruck.Models;

using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AWStruck.Hubs
{
    [HubName("switchHub")]
    public class SwitchHub : Hub
    {
        //  we don't need this as the client is not talking back to server !

        //public void UpdateStatus(InstanceStatus instanceStatus)
        //{
        //    Clients.All.updateInstanceStatus(instanceStatus);
        //}
    }
}
