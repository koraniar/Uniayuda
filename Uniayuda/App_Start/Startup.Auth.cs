using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Uniayuda
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            //Custom provirder create to read language fomr URL
            CookieAuthenticationProvider provider = new CookieAuthenticationProvider();
            var originalHandler = provider.OnApplyRedirect;
            provider.OnApplyRedirect = context =>
            {

                var mvcContext = new HttpContextWrapper(HttpContext.Current);
                var routeData = RouteTable.Routes.GetRouteData(mvcContext);

                //Get the current language  
                RouteValueDictionary routeValues = new RouteValueDictionary();

                //Reuse the RetrunUrl
                //Uri uri = new Uri(context.RedirectUri);
                //string returnUrl = HttpUtility.ParseQueryString(uri.Query)[context.Options.ReturnUrlParameter];
                //routeValues.Add(context.Options.ReturnUrlParameter, returnUrl);
                routeValues.Add("status", "Warning");
                routeValues.Add("message", "You must be logged first.");
                //Overwrite the redirection uri
                UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

                string NewURI = url.Action("Login", "Account", routeValues);

                //Overwrite the redirection uri
                context.RedirectUri = NewURI;
                originalHandler.Invoke(context);
            };

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = provider,
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

        }
    }
}