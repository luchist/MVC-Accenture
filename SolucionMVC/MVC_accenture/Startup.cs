using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_accenture.Startup))]
namespace MVC_accenture
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
