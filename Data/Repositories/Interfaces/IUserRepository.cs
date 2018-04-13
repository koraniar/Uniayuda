using Entities.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.DataProtection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> AddAsync(User user, string password);
        Task<IdentityResult> AddAsync(User user);
        Task<User> GetByIdAsync(string id);
        User GetById(string id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUserNameAsync(string name);
        User GetByUserName(string name);
        Task<User> GetByCredentialsAsync(string userName, string password);
        Task<List<User>> GetAllAsync();
        Task<IdentityResult> ChangePaswordAsync(string userId, string oldPassword, string newPassword);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteASync(User user);
        Task<IdentityResult> AddPasswordAsync(string userId, string newPassword);
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo loginInfo);
        Task<ClaimsIdentity> GetClaimsIdentity(User user);
        Task<ClaimsIdentity> GenerateUserIdentityAsync(User user, string authenticationType);
        Task<User> GetByUserInfoAsync(UserLoginInfo userInfo);
        Task<IdentityResult> RemovePasswordAsync(string userId);
        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo userInfo);
        Task<string> GenerateEmailConfirmationTokenAsync(IDataProtectionProvider provider, string userId);
        Task<string> GeneratePasswordResetTokenAsync(IDataProtectionProvider provider, string userId);
        Task SendEmailAsync(IDataProtectionProvider provider, string userId, string emailSubject, string emailBody);
        Task<IdentityResult> ConfirmEmailAsync(IDataProtectionProvider provider, string userId, string token);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<IdentityResult> ResetPasswordAsync(IDataProtectionProvider provider, string userId, string token, string newPassword);
    }
}
