using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DejtingsajtProjekt.Startup))]
namespace DejtingsajtProjekt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
