using Entities.User;
using Microsoft.AspNetCore.Identity;
using System;

namespace Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (roleManager.FindByNameAsync("Admin").Result == null)
            {
                Role role = new Role()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Description = "Admin Role"
                };

                var result = roleManager.CreateAsync(role).Result;
            }

            var userChecker = userManager.FindByEmailAsync("Admin@ad.min").Result;
            if (userChecker == null)
            {
                User user = new User
                {
                    UserName = "admin",
                    Email = "Admin@ad.min",
                    FullName = "System Admin"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
