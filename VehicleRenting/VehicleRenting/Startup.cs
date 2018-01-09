using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VehicleRenting.Startup))]
namespace VehicleRenting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
