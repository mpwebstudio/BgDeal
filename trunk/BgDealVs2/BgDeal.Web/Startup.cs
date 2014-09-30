using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BgDeal.Web.Startup))]
namespace BgDeal.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
