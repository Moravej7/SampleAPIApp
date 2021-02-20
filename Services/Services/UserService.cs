using Common;
using Common.Exceptions;
using Common.Utilities;
using Data;
using Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService : IUserService
    {
        readonly ApplicationDbContext _userRepository;
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SiteSettings _siteSetting;

        public UserService(ApplicationDbContext userRepository, IJwtService jwtService,
                           UserManager<User> userManager, ILogger<UserService> logger,
                           IHttpContextAccessor httpContextAccessor, SiteSettings siteSettings)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _userManager = userManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _siteSetting = siteSettings;
        }

        public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            var exists = _userRepository.Users.Any(p => p.UserName == user.UserName);
            if (exists)
                throw new BadRequestException("This username already exists");

            var passwordHash = SecurityHelper.GetSha256Hash(password);
            user.PasswordHash = passwordHash;
            await _userRepository.Users.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            _logger.LogInformation($"New User Created . Username : {user.UserName}");
        }

        public async Task<List<User>> Get()
        {
            return await _userRepository.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.Users.SingleOrDefaultAsync(c => c.Id == userId);
                if (user == null)
                    throw new NotFoundException();
                return user;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            return await _userRepository.Users.Where(p => p.UserName == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync();
        }

        public async Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            _userRepository.Users.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<ActionResult> Token(string username, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new BadRequestException("نام کاربری یا کلمه عبور غلط است");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                throw new BadRequestException("نام کاربری یا کلمه عبور غلط است");
            if (user.IsActive == false)
                throw new BadRequestException("کاربر غیر فعال است");

            AccessToken jwt = await _jwtService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        public async Task Update(int id, User updateUser, string Role, CancellationToken cancellationToken)
        {
            User user = await _userRepository.Users.SingleOrDefaultAsync(c => c.Id == id);

            user.UserName = updateUser.UserName;
            user.FullName = updateUser.FullName;
            User CurrentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (updateUser.IsActive == false && id == CurrentUser.Id)
                throw new BadRequestException("نمیتوانید خود را غیر فعال کنید");

            IList<string> userRoles = await _userManager.GetRolesAsync(CurrentUser);

            if (id == CurrentUser.Id && userRoles[0] != Role)
                throw new BadRequestException("نمیتوانید نقش خود را عوض کنید");

            user.IsActive = updateUser.IsActive;
            await _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(user).Result);
            await _userManager.AddToRoleAsync(user, Role);

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Succeeded == false)
            {
                throw new BadRequestException(result.Errors.ToList()[0].Description);
            }
            _logger.LogInformation($"user with username {user.UserName} Updated");
        }

        public async Task ResetPasswordByAdmin(int userId, string newPassword)
        {
            if (newPassword.Length < _siteSetting.IdentitySettings.PasswordRequiredLength)
                throw new BadRequestException($"password length should be greater than {_siteSetting.IdentitySettings.PasswordRequiredLength}");
            if (_siteSetting.IdentitySettings.PasswordRequireDigit && !newPassword.Any(char.IsDigit))
                throw new BadRequestException("Password should contain at least number");
            if (_siteSetting.IdentitySettings.PasswordRequireUppercase && !newPassword.Any(char.IsUpper))
                throw new BadRequestException("Password should contain at least on upper case character");
            if (_siteSetting.IdentitySettings.PasswordRequireLowercase && !newPassword.Any(char.IsLower))
                throw new BadRequestException("PAssword should contain at least on lower case character");
            if (_siteSetting.IdentitySettings.PasswordRequireNonAlphanumic && !(newPassword.Any(char.IsPunctuation) || newPassword.Any(char.IsControl) || newPassword.Any(char.IsSeparator) || newPassword.Any(char.IsSymbol)))
                throw new BadRequestException("Password should at least one NonAlphanumeric character");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new NotFoundException("User not Found");
            }
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join("|", result.Errors.Select(c => c.Description)));
            }
        }

    }
}
