using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;

namespace SelfHosting.Pipeline
{
    //[TestAuthenticationFilter]
    [TestAuthorizationFilter]
    //[Authorize]

    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            ((ClaimsPrincipal)Thread.CurrentPrincipal).Claims.Select(c => c.Value);
            Helper.Write("Controller", Request.GetRequestContext().Principal);

            return Ok();
        }
    }
}
