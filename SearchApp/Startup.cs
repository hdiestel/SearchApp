using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(SearchApp.Startup))]
namespace SearchApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}