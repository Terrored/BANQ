using AutoMapper;
using Owin;

namespace Bootstrapper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());
            ConfigureAuth(app);
        }
    }
}
