using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;

public class Seeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
{
    public async Task<bool> SeedUser()
    {
        var existing = await userManager.FindByNameAsync("admin");
        if (existing != null) return false;

        var user = new User()
        {
            UserName = "admin",
            Email = "admin@gmail.com",
        };
        
        var password = Environment.GetEnvironmentVariable("DEFAULT_USER_PASSWORD");

        var result = await userManager.CreateAsync(user,password);
        if (!result.Succeeded) return false;

        await userManager.AddToRoleAsync(user, Roles.Admin);
        return true;
    }

    public async Task<bool> SeedRole()
    {
        var newroles = new List<IdentityRole>()
        {
            new IdentityRole(Roles.Admin),
            new IdentityRole(Roles.User),
        };

        var roles = await roleManager.Roles.ToListAsync();

        foreach (var role in newroles)
        {
            if (roles.Exists(e => e.Name == role.Name))
            {
                continue;
            }

            await roleManager.CreateAsync(role);
        }

        return true;
    }
}

public static class Roles
{
    public const string Admin = "Admin";
    public const string User = "User";
}