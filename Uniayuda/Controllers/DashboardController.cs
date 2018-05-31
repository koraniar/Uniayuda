using Entities.Entities;
using Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uniayuda.Infraestructure;
using Uniayuda.Models;

namespace Uniayuda.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IPostService _postService;
        private readonly IDatabaseService _databaseService;

        public DashboardController(IPostService postService, IDatabaseService databaseService)
        {
            _postService = postService;
            _databaseService = databaseService;
        }

        public async Task<ActionResult> Index()
        {
            UserSessionData userSD = SessionData.GetUserSessionData();

            if (userSD != null)
            {
                if (string.IsNullOrEmpty(userSD.Name))
                {
                    return RedirectToAction("Profile", "Account", new { fromDashboard = true });
                }

                List<PostViewModel> model = AutoMapperConfiguration._mapper.Map<List<PostViewModel>>((await _postService.GetAllAsync()).OrderBy(x => x.CreatedDate));

                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}