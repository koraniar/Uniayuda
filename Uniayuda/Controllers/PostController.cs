using Entities.DatabaseEntities;
using Entities.Enums;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uniayuda.Infraestructure;
using Uniayuda.Models;

namespace Uniayuda.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IAssessmentService _assessmentService;
        private readonly ICommentService _commentService;
        private readonly IDatabaseService _databaseService;

        public PostController(IPostService postService, IUserService userService, IAssessmentService assessmentService, ICommentService commentService,
            IDatabaseService databaseService)
        {
            _postService = postService;
            _databaseService = databaseService;
            _assessmentService = assessmentService;
            _commentService = commentService;
            _userService = userService;
        }

        public async Task<ActionResult> Index(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                PostViewModel model = AutoMapperConfiguration._mapper.Map<PostViewModel>(await _postService.GetByIdAsync(Guid.Parse(id)));
                if (model != null)
                {
                    model.IsEdition = true;
                    return View(model);
                }
            }
            return View(new PostViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> SavePost(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userService.GetUserByIdAsync(SessionData.GetUserSessionData()?.UserId);
                if (user != null)
                {
                    Post post = null;

                    if (model.IsEdition)
                    {
                        post = await _postService.GetByIdAsync(model.Id);
                        if (post == null)
                        {
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            return Json(new { ResponseStatus = ResponseStatus.Error, Message = "Post not found" }, JsonRequestBehavior.AllowGet);
                        }
                        AutoMapperConfiguration._mapper.Map(model, post);
                        post.EditedDate = DateTime.Now;

                        _postService.Update(post);
                    }
                    else
                    {
                        post = AutoMapperConfiguration._mapper.Map<Post>(model);

                        post.UserId = user.Id;
                        post.User = user;

                        _postService.Create(post);
                    }

                    if (await _databaseService.CommitAsync())
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Message = "Ok" }, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Message = "Error saving data in database" }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Error, Message = "User not found" }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "EMAIL", Message = "Please review the data and try again." }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(Guid postId)
        {
            User user = await _userService.GetUserByIdAsync(SessionData.GetUserSessionData()?.UserId);
            if (user != null && !Guid.Empty.Equals(postId))
            {
                Post post = await _postService.GetByIdAsync(postId);
                if (post != null)
                {
                    int total = 0;
                    Assessment givenAssessment = await _assessmentService.GetByUserIdAndPostIdAsync(user.Id, post.Id);
                    IEnumerable<Assessment> assessments = await _assessmentService.GetAllByPostIdAsync(post.Id);

                    foreach (Assessment assessment in assessments)
                    {
                        total += assessment.Level.GetHashCode();
                    }

                    IEnumerable<CommentViewModel> comments = AutoMapperConfiguration._mapper.Map<IEnumerable<CommentViewModel>>(await _commentService.GetAllByPostIdAsync(postId));

                    PostViewModel model = AutoMapperConfiguration._mapper.Map<PostViewModel>(post);
                    model.AssesmentAverage = (total / (assessments.Count() == 0 ? 1 : assessments.Count()));
                    model.UserAssesment = givenAssessment != null ? givenAssessment.Level.GetHashCode() : 0;
                    model.Comments = comments.ToList();
                    model.UserAuthor = model.IsAnonymous ? "Anonymous" : $"{user.Name} {user.LastName}";

                    return View(model);
                }
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<ActionResult> AddAssessment(Guid postId, int level)
        {
            User user = await _userService.GetUserByIdAsync(SessionData.GetUserSessionData()?.UserId);
            if (user != null && !Guid.Empty.Equals(postId))
            {
                if (level > 0 && level < 6)
                {
                    AssessmentLevel finalLevel = (AssessmentLevel)level;

                    Assessment assessment = await _assessmentService.GetByUserIdAndPostIdAsync(user.Id, postId);

                    if (assessment == null)
                    {
                        assessment = new Assessment()
                        {
                            UserId = user.Id,
                            User = user,
                            PostId = postId,
                            Level = finalLevel
                        };

                        _assessmentService.Create(assessment);
                    }
                    else
                    {
                        assessment.Level = finalLevel;

                        _assessmentService.Update(assessment);
                    }

                    if (await _databaseService.CommitAsync())
                    {
                        int total = 0;
                        IEnumerable<Assessment> assessments = await _assessmentService.GetAllByPostIdAsync(postId);

                        foreach (Assessment item in assessments)
                        {
                            total += item.Level.GetHashCode();
                        }

                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Message = "Ok.", Average = (total / assessments.Count()) }, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Message = "Error saving the assesment." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Error, Message = "Level not valid." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Message = "User not found." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(Guid postId, string newComment)
        {
            User user = await _userService.GetUserByIdAsync(SessionData.GetUserSessionData()?.UserId);
            if (user != null && !Guid.Empty.Equals(postId))
            {
                if (!string.IsNullOrEmpty(newComment))
                {
                    Comment comment = new Comment()
                    {
                        PostId = postId,
                        UserId = user.Id,
                        User = user,
                        Value = newComment
                    };

                    _commentService.Create(comment);

                    if (await _databaseService.CommitAsync())
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Message = "Ok.", Comment = comment.Value,
                            Name = $"{user.Name} {user.LastName}", Date = comment.CreatedDate }, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Message = "Error saving the assesment." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Error, Message = "Level not valid." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Message = "User not found." }, JsonRequestBehavior.AllowGet);
        }
    }
}