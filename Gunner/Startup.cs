using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gunner.Startup))]
namespace Gunner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
