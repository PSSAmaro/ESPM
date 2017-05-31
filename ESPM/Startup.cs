using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ESPM.Startup))]
namespace ESPM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
