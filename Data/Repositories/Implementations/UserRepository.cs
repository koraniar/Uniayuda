using Data.DBInteractions;
using Data.Repositories.Interfaces;
using Entities.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        protected void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
            }
            Dispose(disposing);
        }

        public UserRepository(IDBFactory databaseFactory)
        {
            _userManager = new UserManager<User>(new UserStore<User>(databaseFactory.Get()) { AutoSaveChanges = false });
            _userManager.UserValidator = new UserValidator<User>(_userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            _userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = Cross.Constants.passwordMinimumLength,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };
            _userManager.EmailService = new EmailRepository();
        }

        public async Task<IdentityResult> AddAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<IdentityResult> AddAsync(User user)
        {
            var result = await _userManager.CreateAsync(user);
            return result;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            return result;
        }

        public User GetById(string id)
        {
            var result = _userManager.FindById(id);
            return result;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        public async Task<User> GetByUserNameAsync(string name)
        {
            var result = await _userManager.FindByNameAsync(name);
            return result;
        }

        public User GetByUserName(string name)
        {
            var result = _userManager.FindByName(name);
            return result;
        }

        public async Task<User> GetByCredentialsAsync(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> ChangePaswordAsync(string userId, string oldPassword, string newPassword)
        {
            IdentityResult result = null;
            if (!string.IsNullOrWhiteSpace(oldPassword))
            {
                result = await _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);
            }
            else
            {
                result = await _userManager.RemovePasswordAsync(userId);
                if (result.Succeeded)
                {
                    result = await _userManager.AddPasswordAsync(userId, newPassword);
                }
            }
            return result;
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> DeleteASync(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result;
        }
        public async Task<IdentityResult> AddPasswordAsync(string userId, string newPassword)
        {
            var result = await _userManager.AddPasswordAsync(userId, newPassword);
            return result;
        }
        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo loginInfo)
        {
            var result = await _userManager.AddLoginAsync(userId, loginInfo);
            return result;
        }
        public async Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return identity;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await _userManager.CreateIdentityAsync(user, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        public async Task<User> GetByUserInfoAsync(UserLoginInfo userInfo)
        {
            var result = await _userManager.FindAsync(userInfo);
            return result;
        }

        public async Task<IdentityResult> RemovePasswordAsync(string userId)
        {
            var result = await _userManager.RemovePasswordAsync(userId);
            return result;
        }

        public async Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo userInfo)
        {
            var result = await _userManager.RemoveLoginAsync(userId, userInfo);
            return result;
        }


        public async Task<string> GenerateEmailConfirmationTokenAsync(IDataProtectionProvider provider, string userId)
        {
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create(Cross.Constants.AutofacWebRequest));
            var result = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
            return result;
        }
        public async Task<string> GeneratePasswordResetTokenAsync(IDataProtectionProvider provider, string userId)
        {
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create(Cross.Constants.AutofacWebRequest));
            var result = await _userManager.GeneratePasswordResetTokenAsync(userId);
            return result;
        }

        public async Task SendEmailAsync(IDataProtectionProvider provider, string userId, string emailSubject, string emailBody)
        {
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create(Cross.Constants.AutofacWebRequest));
            await _userManager.SendEmailAsync(userId, emailSubject, emailBody);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(IDataProtectionProvider provider, string userId, string token)
        {
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create(Cross.Constants.AutofacWebRequest));
            var result = await _userManager.ConfirmEmailAsync(userId, token);
            return result;
        }

        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            return await _userManager.IsEmailConfirmedAsync(userId);
        }

        public async Task<IdentityResult> ResetPasswordAsync(IDataProtectionProvider provider, string userId, string token, string newPassword)
        {
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create(Cross.Constants.AutofacWebRequest));
            var result = await _userManager.ResetPasswordAsync(userId, token, newPassword);
            return result;
        }
    }
}
