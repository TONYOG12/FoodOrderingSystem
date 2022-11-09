using Domedo.App.Extensions;
using Domedo.App.Utils;
using Domedo.Domain.Context;
using Domedo.Domain.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Domedo.API.Database.Seeds.TableSeeders
{
    public class UserTableSeeders : ISeeder
    {
        public void Handle(IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetService<DomedoDbContext>();
            var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();

            if (userManager == null || dbContext == null || roleManager == null) return;

            SeedRoles(roleManager, dbContext);
            SeedUsers(userManager, dbContext);

        }

        private static void SeedUsers
         (UserManager<User> userManager, DomedoDbContext dbContext)
        {

            var defaultUser = dbContext.Users.IgnoreQueryFilters()
                .FirstOrDefault(item => item.Email == "super@super.com" || item.UserName == "super@super.com");

            if (defaultUser == null)
            {
                var user = new User
                {
                    UserName = "super@super.com",
                    Email = "super@super.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = userManager.CreateAsync
                    (user, "Pass123$1").Result;

                if (!result.Succeeded) return;

                var roleResult = userManager.AddToRoleAsync(user,
                    RoleUtils.AppRoleSuper).Result;

                if (!roleResult.Succeeded) return;

                var claimResult = userManager.AddClaimsAsync(user, new[]
                {
                    new Claim(JwtClaimTypes.Name, "super super"),
                    new Claim(JwtClaimTypes.GivenName, "super"),
                    new Claim(JwtClaimTypes.FamilyName, "super")
                }).Result;
                Console.WriteLine(claimResult);

            }

        }

        private static void SeedRoles
            (RoleManager<Role> roleManager, DomedoDbContext dbContext)
        {
            foreach (var roleName in RoleUtils.AppRoles())
            {
                var role = dbContext.Roles.IgnoreQueryFilters().FirstOrDefault(item =>
                    item.Name == roleName);
                if (role != null)
                {
                    //AddPermissionsToRole(roleManager, role);
                    Debug.WriteLine("roles being seeded");
                }
                else
                {
                    var newRole = new Role
                    {
                        Name = roleName,
                        NormalizedName = roleName.Capitalize(),
                        DisplayName = roleName.RemoveCharacter('.').Capitalize()
                    };

                    var result = roleManager.CreateAsync(newRole).Result;

                    if (!result.Succeeded) continue;
                    //AddPermissionsToRole(roleManager, newRole);
                }
            }
        }
    }

}
