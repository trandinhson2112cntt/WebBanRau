using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBanRauProject.Startup))]
namespace WebBanRauProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
