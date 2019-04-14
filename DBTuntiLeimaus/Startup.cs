using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DBTuntiLeimaus.Startup))]
namespace DBTuntiLeimaus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
