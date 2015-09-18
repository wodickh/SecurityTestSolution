using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using SelfHosting.Pipeline;

namespace SelfHosting
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseActiveDirectoryFederationServicesBearerAuthentication(
                new ActiveDirectoryFederationServicesBearerAuthenticationOptions
                {
                    MetadataEndpoint = ConfigurationManager.AppSettings["ida:AdfsMetadataEndpoint"],
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = ConfigurationManager.AppSettings["ida:Audience"]
                    }
                });
        }
    }
}