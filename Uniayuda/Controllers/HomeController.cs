using System.Web.Mvc;
using Uniayuda.Infraestructure;

namespace Uniayuda.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var userSD = SessionData.GetUserSessionData();
            if (userSD != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
    }
}