using Coinigy.API;
using Microsoft.Owin;
using Owin;
using static Coinigy.API.Websocket;

[assembly: OwinStartup(typeof(Coinigy.Web.Startup))]

namespace Coinigy.Web
{
    public class Startup
    {
        private static Websocket socket = null;

        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
