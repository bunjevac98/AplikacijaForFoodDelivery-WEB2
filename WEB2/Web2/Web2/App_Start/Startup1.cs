using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using Web2.Models;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(Web2.App_Start.Startup1))]

namespace Web2.App_Start
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.SetDefaultSignInAsAuthenticationType(WsFederationAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType
            });



            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions() {
                ClientId = "724913966855-ap24jj0d9rb1mh3hr5grd0tqfgrvaq6i.apps.googleusercontent.com",
                ClientSecret= "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"
            });
                

            

        }
    }
}
