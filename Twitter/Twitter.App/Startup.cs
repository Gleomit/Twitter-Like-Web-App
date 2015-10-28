using Microsoft.Owin;
using Owin;
using Twitter.App;

[assembly: OwinStartup(typeof (Startup))]

namespace Twitter.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}