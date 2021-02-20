using AutoMapper;
using Common.Exceptions;
using Entities.User;
using Factor.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFramework;
using WebFramework.Api;
using WebFramework.Filters;

namespace SampleApiApp.Controllers
{
    [ApiVersion("1")]
    public class UserController : BaseAPIController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, ILogger<UserController> logger,
            UserManager<User> userManager, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<GetUserDto>>> Get()
        {
            List<User> users = await _userService.Get();
            List<GetUserDto> getUserDtos = new List<GetUserDto>();
            foreach (var user in users)
            {
                var userdto = _mapper.Map<GetUserDto>(user);
                var userRoles = await _userManager.GetRolesAsync(user);
                userdto.Role = userRoles.First();
                getUserDtos.Add(userdto);
            }
            return Ok(getUserDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<GetUserDto>> Get(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            GetUserDto userDto = _mapper.Map<GetUserDto>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userDto.Role = roles.FirstOrDefault();
            return userDto;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Token([FromForm]LoginDto loginDto, CancellationToken cancellationToken)
        {
            if (!loginDto.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
                throw new Exception("OAuth flow is not password.");

            return await _userService.Token(loginDto.username, loginDto.password, cancellationToken);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<CreateUserDto>> Create(CreateUserDto userDto, CancellationToken cancellationToken)
        {
            User user = new User
            {
                FullName = userDto.FullName,
                UserName = userDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded == true)
            {
                await _userManager.AddToRoleAsync(user, userDto.Role);
                return Ok();
            }
            else
            {
                throw new BadRequestException(result.Errors.ToList()[0].Description);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> Update(int id, EditUserDto userDto, CancellationToken cancellationToken)
        {
            User user = new User()
            {
                UserName = userDto.UserName,
                FullName = userDto.FullName,
                IsActive = userDto.IsActive
            };

            await _userService.Update(id, user, userDto.Role, cancellationToken);

            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<ApiResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            string UserId = _userManager.GetUserId(HttpContext.User);
            User user = await _userService.GetByIdAsync(int.Parse(UserId.ToString()));
            IdentityResult result = await _userManager
                .ChangePasswordAsync(user, resetPasswordDto.OldPassword, resetPasswordDto.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                throw new BadRequestException(result.Errors.ToList()[0].Description);
            }
        }

        [HttpPut("[action]/{UserId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> ChangePassword(int UserId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
        {
            await _userService.ResetPasswordByAdmin(UserId, changePasswordDto.NewPassword);
            return Ok();
        }
    }
}