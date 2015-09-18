using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Threading.Tasks;

namespace SelfHosting.Pipeline
{
    public class TestAuthenticationFilterAttribute : Attribute, IAuthenticationFilter
    {
        public async Task AuthenticateAsync(HttpAuthenticationContext context, System.Threading.CancellationToken cancellationToken)
        {
        if (!context.Principal.Identity.IsAuthenticated)
            {
                Helper.Write("AuthenticationFilter", context.ActionContext.RequestContext.Principal);
            }
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
        {
          Debug.WriteLine("AuthenticationFilter:Challenge");  
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}