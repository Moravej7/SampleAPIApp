using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebFramework.Api;

namespace Factor.DataTransferObjects
{
    public class UserDto
    {
        [Required(ErrorMessage = "نام کاربری اجباری است")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "رمز عبور اجباری است")]
        [StringLength(500)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

    }

    public class CreateUserDto
    {
        [Required(ErrorMessage = "نام کاربری اجباری است")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "رمز عبور اجباری است")]
        [StringLength(50)]
        public string Password { get; set; }

        // ConfirmPassword

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public string Role { get; set; }
    }

    public class GetUserDto 
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }
    }

    public class EditUserDto
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }
    }
    public class ResetPasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "رمز عبور با تایید رمز عبور یکسان نیست")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginDto
    {
        [Required]
        public string grant_type { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }

        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}
