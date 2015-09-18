using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Owin;
using SelfHosting.Pipeline;

namespace SelfHosting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.Routes.MapHttpRoute(
                "default", 
                "api/{controller}");
            app.Use(typeof (TestMiddleware));
            app.UseWebApi(configuration);
       //     ConfigureAuth(app);
        }
    }
}
