using Entities.DatabaseEntities;
using Microsoft.Owin.Security.DataProtection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(User user);
        Task<bool> RegisterAsync(User user, string password, IDataProtectionProvider provider);
        Task<User> GetUserByIdAsync(string Id);
        Task<User> GetUserByEmailAsync(string Email);
        Task<User> GetUserByUsernameAsync(string Username);
        User GetUserByUsername(string Username);
        Task<User> GetUserByCredentialsAsync(string email, string password);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByContainsWordInNameAndLastNameAsync(string word);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User purchase);
        Task<ClaimsIdentity> GetClaimsIdentityAsync(User user);
        Task<bool> ChangeEmailAsync(User user, IDataProtectionProvider provider);
        Task<bool> ResendConfirmationEmailAsync(User user, IDataProtectionProvider provider);
        Task<bool> ConfirmEmailAsync(IDataProtectionProvider provider, string userId, string token);
        Task SendConfirmationEmailAsync(User user, IDataProtectionProvider provider);
        Task<bool> ChangePasswordAsync(string userId, string newPassword, string oldPassword = null);
        Task<bool> SendResetPasswordEmailAsync(IDataProtectionProvider provider, User user);
        Task<bool> ResetPasswordAsync(IDataProtectionProvider provider, User user, string token, string newPassword);
    }
}
