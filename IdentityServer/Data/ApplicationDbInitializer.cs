using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data
{
    public class ApplicationDbInitializer
    {
        public static   void SeedUsers(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (roleManager.FindByNameAsync("Admin").Result == null)
            {
                IdentityRole role = new IdentityRole
                {
                    Name="Admin" 
                };

                 var result=  roleManager.CreateAsync(role).Result; 

                
            }

            if (userManager.FindByEmailAsync("admin@abc.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin1",
                    Email = "admin@abc.com"
                };
                
                IdentityResult result = userManager.CreateAsync(user, "Pa$$word123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
