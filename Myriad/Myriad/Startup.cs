using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Myriad.Startup))]
namespace Myriad
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
