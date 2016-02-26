using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NCD.Startup))]
namespace NCD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
