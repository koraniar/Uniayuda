using Autofac;
using Entities.DatabaseEntities;
using Entities.Enums;
using Logic.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Uniayuda.Infraestructure;
using Uniayuda.Models;

namespace Uniayuda.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDatabaseService _databaseService;

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public AccountController(IUserService userService, IDatabaseService databaseService)
        {
            _userService = userService;
            _databaseService = databaseService;
        }

        public async Task<ActionResult> Index()
        {
            var userSD = SessionData.GetUserSessionData();
            string userId = SessionData.GetUserSessionData()?.UserId;
            if (userSD != null && string.IsNullOrEmpty(userSD.Name))
            {
                return RedirectToAction("Profile", "Account", new { fromDashboard = true });
            }

            User user = string.IsNullOrEmpty(userId) ? null : await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                ProfileViewModel model = AutoMapperConfiguration._mapper.Map<ProfileViewModel>(user);

                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        public new async Task<ActionResult> Profile(bool fromDashboard = false)
        {
            //var userSD = SessionData.GetUserSessionData();
            string userId = SessionData.GetUserSessionData()?.UserId;
            //if (userSD != null && userSD.EmailConfirmed == false)
            //{
            //    return RedirectToAction("UnconfirmedEmail");
            //}

            User user = string.IsNullOrEmpty(userId) ? null : await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                ProfileViewModel model = AutoMapperConfiguration._mapper.Map<ProfileViewModel>(user);
                model.IsFromDashboard = fromDashboard;
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSaveProfile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userService.GetUserByIdAsync(SessionData.GetUserSessionData().UserId);
                if (user != null)
                {
                    AutoMapperConfiguration._mapper.Map(model, user);
                    user.BornDate = null;//BORN DATE

                    bool result = await _userService.UpdateUserAsync(user);
                    result = result ? await _databaseService.CommitAsync() : false;

                    if (result)
                    {
                        if (model.IsFromDashboard)
                        {
                            SessionData.GetAndRestoreUserSessionData();
                            return RedirectToAction("Index", "Dashboard");
                        }
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Profile", "Account", new { fromDashboard = true });
        }

        #region Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
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
                        return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "PASSWORD", Message = $"La contraseña debe contener minimo {Cross.Constants.passwordMinimumLength} caracteres." }, JsonRequestBehavior.AllowGet);
                    }

                    if (await _userService.GetUserByEmailAsync(model.Email) != null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "EMAIL", Message = "Este correo ya esta registrado." }, JsonRequestBehavior.AllowGet);
                    }

                    User user = AutoMapperConfiguration._mapper.Map<User>(model);

                    if (await _userService.RegisterAsync(user, model.Password, IoCConfig.ApplicationContainer.Container.Resolve<IDataProtectionProvider>()))
                    {
                        await SignInAsync(user, true);
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(new { ResponseStatus = ResponseStatus.Success, Error = "", Message = "." }, JsonRequestBehavior.AllowGet);
                    }

                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Por favor intente de nuevo mas tarde." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "PASSWORD", Message = "Las contraseñas no coinciden." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Por favor valide los datos ingresados." }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmailConfirmation
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
                                SessionData.GetAndRestoreUserSessionData();
                                return RedirectToAction("Index", "Dashboard");
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
                        return RedirectToAction("Index", "Dashboard");
                    }
                    model.Email = userSD.EmailAddress;
                    model.UserName = userSD.UserName;
                }
            }
            return View(model);
        }
        #endregion

        #region ChangeEmailPassword
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

        public ActionResult Settings()
        {
            //var userSD = SessionData.GetUserSessionData();
            //if (userSD != null && userSD.EmailConfirmed == false)
            //{
            //    return RedirectToAction("UnconfirmedEmail");
            //}

            return View();
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
        #endregion

        #region RestorePassword
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
                User user = await _userService.GetUserByEmailAsync(model.EmailOrUsername.Trim());

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
                return RedirectToAction("Index", "Dashboard");
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
        #endregion

        #region LoginLogout
        [AllowAnonymous]
        public ActionResult Login(ResponseStatus status = ResponseStatus.None, string message = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
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
                User fakeUser = await _userService.GetUserByEmailAsync(model.UsernameEmail.Trim());
                if (fakeUser != null)
                {
                    user = await _userService.GetUserByCredentialsAsync(fakeUser.UserName, model.Password);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "ACCOUNT", Message = "La combinación de correo y contraseña no es válida." }, JsonRequestBehavior.AllowGet);
                }

                if (user != null)
                {
                    await SignInAsync(user, true);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { ResponseStatus = ResponseStatus.Success, Error = "", Message = "Entrando." }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { ResponseStatus = ResponseStatus.Warning, Error = "CREDENTIALS", Message = "La combinación de correo y contraseña no es válida." }, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { ResponseStatus = ResponseStatus.Error, Error = "VALID", Message = "Por favor valide la información que ingreso." }, JsonRequestBehavior.AllowGet);
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
