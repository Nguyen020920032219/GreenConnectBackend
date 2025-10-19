using Microsoft.AspNetCore.Identity;

namespace GreenConnectPlatform.Api.Configurations;

public class DataSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        string[] roleNames = { "Admin", "Household", "ScrapCollector" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                Console.WriteLine($"[DataSeeder] Created role: {roleName}");
            }
        }
    }
}