using Microsoft.AspNetCore.Identity;
using ProjectName.Auth.Application.Enums;
using ProjectName.Auth.Domain.Entities.Identity;

namespace ProjectName.Auth.Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new Role(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new Role(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new Role(Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new Role(Roles.Basic.ToString()));
        }
    }
}
