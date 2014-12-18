using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AWStruck.Hub
{
    public class HublyBubly : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}