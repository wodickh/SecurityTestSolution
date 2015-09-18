using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace SelfHosting.Pipeline
{
    public class TestMiddleware
    {
        private Func<IDictionary<string, object>, Task> _next;
        public TestMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);
            // authentication
            JwtSecurityTokenHandler tokenHandler  = new JwtSecurityTokenHandler();
            Debug.WriteLine("authentication middleware" + context.Authentication.ToString());
            string requestToken = null;
            string authorization = context.Request.Headers.Get("Authorization");
            if (!string.IsNullOrEmpty(authorization))
            {
                if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    requestToken = authorization.Substring("Bearer ".Length).Trim();
                }
            }
            this.UnprotectJWT(requestToken);

            Helper.Write("Middleware", context.Request.User);

            await _next(env);
        }

        private void UnprotectJWT(string jwt)
        {
            JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
            var token = TokenHandler.ReadToken(jwt) as JwtSecurityToken;

        }
    }
}
