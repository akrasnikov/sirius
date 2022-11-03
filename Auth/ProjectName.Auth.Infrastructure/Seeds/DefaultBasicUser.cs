using Microsoft.AspNetCore.Identity;
using ProjectName.Auth.Application.Enums;
using ProjectName.Auth.Domain.Entities.Identity;

namespace ProjectName.Auth.Infrastructure.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Seed Default User
            var defaultUser = new User
            {
                UserName = "basicuser",
                Email = "basicuser@gmail.com",
                FirstName = "John",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }

            }
        }
    }
}
