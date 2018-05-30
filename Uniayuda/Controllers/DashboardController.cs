using Entities.Entities;
using System.Web.Mvc;
using Uniayuda.Infraestructure;

namespace Uniayuda.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            UserSessionData userSD = SessionData.GetUserSessionData();

            if (userSD != null)
            {
                if (string.IsNullOrEmpty(userSD.Name))
                {
                    return RedirectToAction("Profile", "Account", new { fromDashboard = true });
                }
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}