using IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace helpdesk_backend.Identity
{
    public class DBInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            string[] roleNames = { "Owner" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    var roleResult = await _roleManager.CreateAsync(new Role(roleName));
                }
            }
            PasswordHasher<string> pw = new PasswordHasher<string>();

            var owner = new User
            {
                UserName = "owner",
                Email = "test@test.ru",
                PasswordHash = new PasswordHasher<string>().HashPassword("owner", "owner"),
            };

            var _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var result = await _userManager.FindByNameAsync(owner.UserName);
            if (result == null) {
                await _userManager.CreateAsync(owner);
                var identity = _userManager.AddToRoleAsync(owner, "Owner");
            }
            //if (!context.Users.Any(u => u.UserName == owner.UserName))
            //{
            //}


            //base.Seed(context);
        }
    }
}
