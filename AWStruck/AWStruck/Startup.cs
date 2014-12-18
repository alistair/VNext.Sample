using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire;
using Hangfire.Mongo;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AWStruck.Startup))]

namespace AWStruck
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
			app.UseHangfire(
				configuration =>
				{
					configuration.UseServer();
					configuration.UseStorage(new MongoStorage("mongodb://localhost/", "aswtruck"));
				}
				);

	        BackgroundJob.Schedule(() => AWSTasks.StopInstancesTask("mytest"), DateTimeOffset.UtcNow);
			
        }
    }
}
