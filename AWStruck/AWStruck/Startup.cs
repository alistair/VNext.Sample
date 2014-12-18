using AWStruck;

using Hangfire;
using Hangfire.Mongo;

using Microsoft.Owin;

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
                    configuration.UseStorage(new MongoStorage("mongodb://localhost/", "aswtruck"));
                    configuration.UseDashboardPath("/hangfire");
                });

            app.MapSignalR();
        }
    }
}
