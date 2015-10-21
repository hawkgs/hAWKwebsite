using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(hAWK.Web.Startup))]
namespace hAWK.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
