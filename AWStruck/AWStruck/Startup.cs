using AWStruck;

using Hangfire;
using Hangfire.Mongo;

using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;

using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace AWStruck
{
    public partial class Startup
    {
        public static string MONGO_CONNECTION_STRING;
        public static string MONGO_DATABASE;

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseHangfire(
                configuration =>
                {
                    configuration.UseServer();
                    configuration.UseStorage(new MongoStorage("mongodb://54.69.155.172/", "aswtruck"));
                    configuration.UseDashboardPath("/hangfire");
                });

            //   app.MapSignalR();
            app.Map(
                "/signalr",
                map =>
                {
                    map.UseCors(CorsOptions.AllowAll);
                    var hubConfiguration = new HubConfiguration
                    {
                        EnableJSONP = true
                    };
                    map.RunSignalR(hubConfiguration);
                });
        }
    }
}
