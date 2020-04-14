using Microsoft.AspNetCore.Identity;
using System.Linq;
using TodoProject.General;
using TodoProject.Models;

namespace TodoProject.Data
{
    public class SeedData
    {
        public static void Seed(RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedCategory(context);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(RoleNames.USER).Result)
            {
                var role = new IdentityRole()
                {
                    Name = RoleNames.USER
                };

                roleManager.CreateAsync(role).Wait();
            }

            if(!roleManager.RoleExistsAsync(RoleNames.ADMIN).Result)
            {
                var role = new IdentityRole()
                {
                    Name = RoleNames.ADMIN
                };

                roleManager.CreateAsync(role).Wait();
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var password = "Test@123";
            var email = "user@todo.com";
            
            var user = userManager.FindByEmailAsync(email).Result;
            if (user == null)
            {
                var firstName = "Default";
                var surname = "User";
                user = new ApplicationUser()
                {
                    Email = email,
                    UserName = $"{firstName[0]}{surname}",
                    FirstName = firstName,
                    Surname = surname
                };

                var created = userManager.CreateAsync(user, password).Result;
                if (created.Succeeded)
                {
                    userManager.AddToRoleAsync(user, RoleNames.USER).Wait();
                }
            }

            email = "admin@todo.com";
            user = userManager.FindByEmailAsync(email).Result;
            if(user == null)
            {
                var firstName = "Default";
                var surname = "Admin";
                user = new ApplicationUser()
                {
                    Email = email,
                    UserName = $"{firstName[0]}{surname}",
                    FirstName = firstName,
                    Surname = surname
                };

                var created = userManager.CreateAsync(user, password).Result;
                if (created.Succeeded)
                {
                    userManager.AddToRoleAsync(user, RoleNames.USER).Wait();
                    userManager.AddToRoleAsync(user, RoleNames.ADMIN).Wait();
                }
            }
        }

        private static void SeedCategory(ApplicationDbContext context)
        {
            var defaultName = "Default";
            var category = context.Categories
                .Where(x => x.Name.Equals(defaultName))
                .FirstOrDefault();

            if(category == null)
            {
                category = new Category()
                {
                    Name = defaultName
                };

                context.Categories.Add(category);
                context.SaveChanges();
            }
        }
    }
}
