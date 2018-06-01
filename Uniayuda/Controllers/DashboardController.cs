using Entities.DatabaseEntities;
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
        private readonly ICommentService _commentService;
        private readonly IAssessmentService _assessmentService;
        private readonly IDatabaseService _databaseService;

        public DashboardController(IPostService postService, ICommentService commentService, IAssessmentService assessmentService, IDatabaseService databaseService)
        {
            _postService = postService;
            _commentService = commentService;
            _assessmentService = assessmentService;
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

                List<PostViewModel> model = AutoMapperConfiguration._mapper.Map<List<PostViewModel>>((await _postService.GetAllAsync()).OrderByDescending(x => x.CreatedDate));

                foreach (PostViewModel item in model)
                {
                    double total = 0;
                    IEnumerable<Assessment> assessments = await _assessmentService.GetAllByPostIdAsync(item.Id);

                    foreach (Assessment assessment in assessments)
                    {
                        total += assessment.Level.GetHashCode();
                    }

                    item.Comments = AutoMapperConfiguration._mapper.Map<List<CommentViewModel>>((await _commentService.GetLastCommentsByPostIdAsync(item.Id, 3))
                        .ToList().OrderBy(x => x.CreatedDate));
                    item.AssesmentAverage = (total / (assessments.Count() == 0 ? 1d : double.Parse(assessments.Count().ToString())));
                }

                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}