using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Menajes_Maipu.Startup))]
namespace Menajes_Maipu
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
