using Uniayuda.Infraestructure;
using Uniayuda.Models;
using Autofac;
using Entities.Entities;
using Entities.Enums;
using Logic.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Uniayuda.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;
        private readonly IProfessionService _professionService;
        private readonly IPhotoService _photoService;
        private readonly IDatabaseService _databaseService;

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public AccountController(IUserService userService, ICountryService countryService, IProfessionService professionService, IPhotoService photoService,
            IDatabaseService databaseService)
        {
            _userService = userService;
            _countryService = countryService;
            _professionService = professionService;
            _photoService = photoService;
            _databaseService = databaseService;
        }

        public async Task<ActionResult> Index()
        {
            var userSD = SessionData.GetUserSessionData();
            string userId = userSD?.UserId;
            if (userSD != null && userSD.EmailConfirmed == false)
            {
                return RedirectToAction("UnconfirmedEmail");
            }

            User user = string.IsNullOrEmpty(userId) ? null : await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                List<Country> CountriesList = (await _countryService.GetAllCountriesAsync()).ToList();
                List<Profession> ProfessionsList = (await _professionService.GetAllProfessionsAsync()).ToList();

                ProfileViewModel model = AutoMapperConfiguration._mapper.Map<ProfileViewModel>(user);

                foreach (var item in CountriesList)
                {
                    model.Countries.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }
                foreach (var item in ProfessionsList)
                {
                    model.Professions.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        public new async Task<ActionResult> Profile()
        {
            var userSD = SessionData.GetUserSessionData();
            string userId = userSD?.UserId;

            if (userSD != null && userSD.EmailConfirmed == false)
            {
                return RedirectToAction("UnconfirmedEmail");
            }

            User user = string.IsNullOrEmpty(userId) ? null : await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                ProfileViewModel model = AutoMapperConfiguration._mapper.Map<ProfileViewModel>(user);

                List<Country> CountriesList = (await _countryService.GetAllCountriesAsync()).ToList();
                List<Profession> ProfessionsList = (await _professionService.GetAllProfessionsAsync()).ToList();
                foreach (Country item in CountriesList)
                {
                    model.Countries.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }
                foreach (Profession item in ProfessionsList)
                {
                    model.Professions.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }

                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSaveProfile(ProfileViewModel model)
        {
            User user = await _userService.GetUserByIdAsync(SessionData.GetUserSessionData().UserId);
            if (user != null)
            {
                AutoMapperConfiguration._mapper.Map(model, user);

                if (model.URLPhoto != null && (!user.Photos.Any(x => x.Active) || model.URLPhoto != user.Photos.FirstOrDefault(x => x.Active).Path))
                {
                    foreach (Photo item in user.Photos.Where(x => x.Active))
                    {
                        item.Active = false;
                        _photoService.UpdatePhoto(item);
                    }

                    Photo photo = new Photo()
                    {
                        Id = Guid.NewGuid(),
                        Format = PhotoFormatType.Image,
                        Path = model.URLPhoto,
                        Active = true,
                        CreatedDate = DateTime.Now,
                        UserId = user.Id
                    };
                    _photoService.CreatePhoto(photo);
                }

                bool result = await _databaseService.CommitAsync();

                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password == model.RepeatPassword)
                {
                    if (model.Password.Length < Cross.Constants.passwordMinimumLength)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "PASSWORD", Message = "The password must be contains at least 8 characters." }, JsonRequestBehavior.AllowGet);
                    }


                    User existentUser = await _userService.GetUserByEmailAsync(model.Email);
                    if (existentUser != null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "EMAIL", Message = "This email is already registered." }, JsonRequestBehavior.AllowGet);
                    }

                    existentUser = await _userService.GetUserByUsernameAsync(model.UserName);
                    if (existentUser != null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "USERNAME", Message = "This username is already taken." }, JsonRequestBehavior.AllowGet);
                    }

                    User user = AutoMapperConfiguration._mapper.Map<User>(model);

                    var result = await _userService.RegisterAsync(user, model.Password, IoCConfig.ApplicationContainer.Container.Resolve<IDataProtectionProvider>());
                    if (result)
                    {
                        await SignInAsync(user, true);
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Error = "", Message = "." }, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Please try again later." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "PASSWORD", Message = "The passwords does not match." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Please validate entered data." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResendConfirmationEmail()
        {
            string userId = SessionData.GetUserSessionData()?.UserId;
            User user = string.IsNullOrEmpty(userId) ? null : await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                try
                {
                    bool result = await _userService.ResendConfirmationEmailAsync(user, IoCConfig.ApplicationContainer.Container.Resolve<IDataProtectionProvider>());
                    if (result)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Error = "", Message = $"We resend you an email to <strong>{user.Email}</strong>, if you not receive the email after 5 minutes, please review on spam folder" }, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Please try again later." }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "TIME", Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Please try again later." }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Id, string token)
        {
            User user = string.IsNullOrEmpty(Id) ? null : await _userService.GetUserByIdAsync(Id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(token))
                {
                    token = token.Replace(" ", "+");
                    bool result = await _userService.ConfirmEmailAsync(IoCConfig.ApplicationContainer.Container.Resolve<IDataProtectionProvider>(), user.Id, token);
                    if (result)
                    {
                        user.EmailConfirmed = true;
                        result = await _userService.UpdateUserAsync(user);
                        result = result ? await _databaseService.CommitAsync() : false;

                        if (result)
                        {
                            if (Request.IsAuthenticated)
                            {
                                var userData = SessionData.GetAndRestoreUserSessionData();
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return RedirectToAction("Login", new { status = ResponseStatus.Success, message = "Your email has been confirmed, log in to access your profile." });
                            }
                        }
                        return RedirectToAction("UnconfirmedEmail", new { status = ResponseStatus.Warning, errorMessage = "We cannot update this user, try again later." });
                    }
                    return RedirectToAction("UnconfirmedEmail", new { status = ResponseStatus.Warning, errorMessage = "Error confirming your email, maybe this link is very old, try again with another link." });
                }
                return RedirectToAction("UnconfirmedEmail", new { status = ResponseStatus.Warning, errorMessage = "Error confirming your email, this link is not valid." });
            }
            return RedirectToAction("UnconfirmedEmail", new { status = ResponseStatus.Error, errorMessage = "Unexpected Error." });
        }

        [AllowAnonymous]
        public ActionResult UnconfirmedEmail(ResponseStatus status = ResponseStatus.None, string errorMessage = "")
        {
            UnconfirmedEmailViewModel model = new UnconfirmedEmailViewModel()
            {
                errorCode = status.GetHashCode(),
                errorMessage = errorMessage
            };
            if (Request.IsAuthenticated)
            {
                var userSD = SessionData.GetAndRestoreUserSessionData();
                if (userSD != null)
                {
                    if (userSD.EmailConfirmed)
                    {
                        return RedirectToAction("Index");
                    }
                    model.Email = userSD.EmailAddress;
                    model.UserName = userSD.UserName;
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangeEmail(string email)
        {
            return View("_changeEmail", new ChangeEmailViewModel()
            {
                Email = string.IsNullOrEmpty(email) ? string.Empty : email,
                RepeatEmail = string.IsNullOrEmpty(email) ? string.Empty : email,
                hideSubmitButton = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = SessionData.GetUserSessionData()?.UserName;
                User user = await _userService.GetUserByCredentialsAsync(userName, model.Password);

                if (user != null)
                {
                    User existentUser = await _userService.GetUserByEmailAsync(model.Email);
                    if (existentUser != null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "EMAIL", Message = "This email is already registered." }, JsonRequestBehavior.AllowGet);
                    }

                    user.Email = model.Email;
                    user.EmailConfirmed = false;
                    user.LastEmailResended = null;

                    if (await _userService.ChangeEmailAsync(user, IoCConfig.ApplicationContainer.Container.Resolve<IDataProtectionProvider>()))
                    {
                        SessionData.GetAndRestoreUserSessionData();
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Error = "", Message = $"we have sent the confirmation email to your new email (<strong>{model.Email}</strong>)." }, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Error updating the user, please try again later." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "VALID", Message = "The password is not valid." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Please validate entered data." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassWord(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = SessionData.GetUserSessionData()?.UserName;
                User user = await _userService.GetUserByCredentialsAsync(userName, model.CurrentPassword);

                if (user != null)
                {
                    string UserId = User.Identity.GetUserId();
                    bool result = await _userService.ChangePasswordAsync(UserId, model.NewPassword, model.CurrentPassword);
                    if (result)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Error = "", Message = "Your password has been changed." }, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Error updating the user, please try again later." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "VALID", Message = "The password is not valid." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Please validate entered data." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ShowResetPassword()
        {
            return View("_resetPassword", new FillForgotPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(FillForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                if (model.EmailOrUsername.Contains("@"))
                {
                    user = await _userService.GetUserByEmailAsync(model.EmailOrUsername.Trim());
                }
                else
                {
                    user = await _userService.GetUserByUsernameAsync(model.EmailOrUsername);
                }

                if (user != null)
                {
                    bool result = await _userService.SendResetPasswordEmailAsync(IoCConfig.ApplicationContainer.Container.Resolve<IDataProtectionProvider>(), user);
                    if (result)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Error = "", Message = "We have sent you an email to restore your password." }, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "please try again later." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "ACCOUNT", Message = "This email is not associated to any account." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Please validate entered data." }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string Id, string token)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            User user = string.IsNullOrEmpty(Id) ? null : await _userService.GetUserByIdAsync(Id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(token))
                {
                    return View(new ForgotPasswordViewModel() { Token = token, UserId = Id });
                }
            }
            return View(new ForgotPasswordViewModel() { ErrorMessage = "This link is not valid, try with another link." });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RestorePassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userService.GetUserByIdAsync(model.UserId);
                if (user != null)
                {
                    bool restored = await _userService.ResetPasswordAsync(IoCConfig.ApplicationContainer.Container.Resolve<IDataProtectionProvider>(), user, model.Token, model.NewPassword);
                    if (restored)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Error = "", Message = "Your password has been changed." }, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Error = "TOKEN", Message = "We cannot update the user, maybe this link is very old, please try again with another link." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Error, Error = "TOKEN", Message = "This link is not valid." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Please validate entered data." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Settings()
        {
            var userSD = SessionData.GetUserSessionData();

            if (userSD != null && userSD.EmailConfirmed == false)
            {
                return RedirectToAction("UnconfirmedEmail");
            }

            return View();
        }

        #region Login
        [AllowAnonymous]
        public ActionResult Login(ResponseStatus status = ResponseStatus.None, string message = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View(new LoginViewModel() { statusCode = status.GetHashCode(), statusMessage = message });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginUser(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                if (model.UsernameEmail.Contains("@"))
                {
                    User fakeUser = await _userService.GetUserByEmailAsync(model.UsernameEmail.Trim());
                    if (fakeUser != null)
                    {
                        user = await _userService.GetUserByCredentialsAsync(fakeUser.UserName, model.Password);
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "ACCOUNT", Message = "This account does not exist." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    User fakeUser = await _userService.GetUserByUsernameAsync(model.UsernameEmail);
                    if (fakeUser != null)
                    {
                        user = await _userService.GetUserByCredentialsAsync(model.UsernameEmail, model.Password);
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "ACCOUNT", Message = "This account does not exist." }, JsonRequestBehavior.AllowGet);
                    }
                }

                if (user != null)
                {
                    await SignInAsync(user, true);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Success, Error = "", Message = "Logged In." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "CREDENTIALS", Message = "The combination of email and password is not correct." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Please validate entered data." }, JsonRequestBehavior.AllowGet);
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            ClaimsIdentity identity = await _userService.GetClaimsIdentityAsync(user);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            SessionData.AbandonSession();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
