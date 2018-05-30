using Entities.DatabaseEntities;
using Entities.Enums;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Uniayuda.Infraestructure;
using Uniayuda.Models;

namespace Uniayuda.Controllers
{
    public class PostController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IDatabaseService _databaseService;

        public PostController(IPostService postService, IUserService userService, IDatabaseService databaseService)
        {
            _postService = postService;
            _databaseService = databaseService;
            _userService = userService;
        }

        public ActionResult Index()
        {
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
    }
}