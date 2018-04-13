using Data.DBInteractions;
using Data.Repositories.Interfaces;
using Entities.Entities;
using Logic.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userManagerRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userManagerRepository, IGenericRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _userManagerRepository = userManagerRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            var result = await _userManagerRepository.AddAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> RegisterAsync(User user, string password, IDataProtectionProvider provider)
        {
            IdentityResult result = await _userManagerRepository.AddAsync(user, password);
            if (result.Succeeded)
            {
                var commit = await _unitOfWork.CommitAsync();
                if (commit)
                {
                    await SendConfirmationEmailAsync(user, provider);
                }
                return commit;
            }
            return result.Succeeded;
        }

        public async Task<User> GetUserByIdAsync(string Id)
        {
            return await _userManagerRepository.GetByIdAsync(Id);
        }

        public async Task<User> GetUserByEmailAsync(string Email)
        {
            return await _userManagerRepository.GetByEmailAsync(Email);
        }

        public async Task<User> GetUserByUsernameAsync(string Username)
        {
            return await _userManagerRepository.GetByUserNameAsync(Username);
        }

        public User GetUserByUsername(string Username)
        {
            return _userManagerRepository.GetByUserName(Username);
        }

        public async Task<User> GetUserByCredentialsAsync(string email, string password)
        {
            var result = await _userManagerRepository.GetByCredentialsAsync(email, password);
            return result;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userManagerRepository.GetAllAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByContainsWordInNameAndLastNameAsync(string word)
        {
            return await _userRepository.GetManyAsync(k => k.Name.ToLower().Contains(word.ToLower()) || k.LastName.ToLower().Contains(word.ToLower()));
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var result = await _userManagerRepository.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            var result = await _userManagerRepository.DeleteASync(user);
            return result.Succeeded;
        }

        public async Task<ClaimsIdentity> GetClaimsIdentityAsync(User user)
        {
            return await _userManagerRepository.GetClaimsIdentity(user);
        }

        public async Task<bool> ChangeEmailAsync(User user, IDataProtectionProvider provider)
        {
            var result = await UpdateUserAsync(user);

            if (result)
            {
                var commit = await _unitOfWork.CommitAsync();
                if (commit)
                {
                    await SendConfirmationEmailAsync(user, provider);
                }
                return commit;
            }
            return result;
        }

        public async Task<bool> ResendConfirmationEmailAsync(User user, IDataProtectionProvider provider)
        {
            if (user.LastEmailResended.HasValue)
            {
                if (DateTime.Now.AddHours(-1).CompareTo(user.LastEmailResended.Value) < 0)
                {
                    throw new Exception("It's too early to try again, do not forget to check your spam folder.");
                }
            }
            user.LastEmailResended = DateTime.Now;
            var result = await UpdateUserAsync(user);

            if (result)
            {
                var commit = await _unitOfWork.CommitAsync();
                if (commit)
                {
                    await SendConfirmationEmailAsync(user, provider);
                }
                return commit;
            }
            return result;
        }

        public async Task<bool> ConfirmEmailAsync(IDataProtectionProvider provider, string userId, string token)
        {
            var result = await _userManagerRepository.ConfirmEmailAsync(provider, userId, token);
            return result.Succeeded ? _unitOfWork.Commit() : false;
        }

        public async Task SendConfirmationEmailAsync(User user, IDataProtectionProvider provider)
        {
            string token = await _userManagerRepository.GenerateEmailConfirmationTokenAsync(provider, user.Id);
            token = System.Web.HttpUtility.UrlEncode(token);
            token = token.Replace(" ", "+");
            var callbackUrl = $"{ConfigurationManager.AppSettings[Cross.Constants.UrlEnvironment]}/Account/ConfirmEmail?Id={user.Id}&token={token}";
            await _userManagerRepository.SendEmailAsync(provider, user.Id, "Confirm your email on Uniayuda.com.", "Please confirm your account by clicking <a href='"
               + callbackUrl + "'>here</a>");
        }

        public async Task<bool> ChangePasswordAsync(string userId, string newPassword, string oldPassword = null)
        {
            var result = await _userManagerRepository.ChangePaswordAsync(userId, oldPassword, newPassword);
            if (result.Succeeded)
            {
                await _unitOfWork.CommitAsync();
            }
            return result.Succeeded;
        }

        public async Task<bool> SendResetPasswordEmailAsync(IDataProtectionProvider provider, User user)
        {
            string token = await _userManagerRepository.GeneratePasswordResetTokenAsync(provider, user.Id);
            token = System.Web.HttpUtility.UrlEncode(token);
            var callbackUrl = $"{ConfigurationManager.AppSettings[Cross.Constants.UrlEnvironment]}/Account/ResetPassword?Id={user.Id}&token={token}";
            await _userManagerRepository.SendEmailAsync(provider, user.Id, "Reset your password on Uniayuda.com.", "Change your password by clicking <a href='"
               + callbackUrl + "'>here</a>");
            return true;
        }

        public async Task<bool> ResetPasswordAsync(IDataProtectionProvider provider, User user, string token, string newPassword)
        {
            var changeResult = await _userManagerRepository.ResetPasswordAsync(provider, user.Id, token, newPassword);
            
            if (changeResult.Succeeded)
            {
                user.LastTimePasswordRestored = DateTime.Now;
                await UpdateUserAsync(user);
                return _unitOfWork.Commit(); 
            }
            return changeResult.Succeeded;
        }
    }
}
