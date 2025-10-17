using Microsoft.AspNetCore.Identity;

namespace GreenConnectPlatform.Api.Configurations;

public class DataSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        // Lấy RoleManager từ service provider
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // Danh sách các vai trò cần có trong hệ thống
        string[] roleNames = { "Admin", "Household", "ScrapCollector" };

        foreach (var roleName in roleNames)
        {
            // Kiểm tra xem vai trò đã tồn tại chưa
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Nếu chưa, tạo vai trò mới
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                Console.WriteLine($"[DataSeeder] Created role: {roleName}");
            }
        }
    }
}