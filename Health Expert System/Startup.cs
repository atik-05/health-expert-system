using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Health_Expert_System.Startup))]
namespace Health_Expert_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
