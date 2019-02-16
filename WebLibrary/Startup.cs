using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebLibrary.Startup))]
namespace WebLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
