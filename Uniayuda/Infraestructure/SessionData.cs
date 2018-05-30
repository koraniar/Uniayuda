using Autofac;
using Cross;
using Entities.DatabaseEntities;
using Entities.Entities;
using Logic.Interfaces;
using System.Web;

namespace Uniayuda.Infraestructure
{
    public class SessionData
    {
        public static UserSessionData GetUserSessionData()
        {
            if (HttpContext.Current.Session == null || HttpContext.Current.Session[Cross.Constants.UserSessionDataKey] == null)
            {
                using (var scope = IoCConfig.ApplicationContainer.Container.BeginLifetimeScope(Cross.Constants.AutofacWebRequest))
                {
                    IUserService userService = scope.Resolve<IUserService>();
                    var username = HttpContext.Current.User.Identity.Name;
                    User user = userService.GetUserByUsername(username);
                    if (user == null)
                    {
                        HttpContext.Current.GetOwinContext().Authentication.SignOut();
                        return null;
                    }

                    UserSessionData sessionData = new UserSessionData()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Name = user.Name,
                        EmailAddress = user.Email,
                        EmailConfirmed = user.EmailConfirmed
                    };

                    HttpContext.Current.Session.Add(Constants.UserSessionDataKey, sessionData);
                    return sessionData;
                }
            }
            else
            {
                var val = HttpContext.Current.Session[Constants.UserSessionDataKey];
                return (UserSessionData)val;
            }
        }

        public static UserSessionData GetAndRestoreUserSessionData()
        {
            if (HttpContext.Current.Session == null && HttpContext.Current.Session[Constants.UserSessionDataKey] == null)
            {
                return GetUserSessionData();
            }
            else
            {
                HttpContext.Current.Session.Remove(Constants.UserSessionDataKey);
                return GetUserSessionData();
            }
        }

        public static void AbandonSession()
        {
            HttpContext.Current.Session.Abandon();
        }
    }
}