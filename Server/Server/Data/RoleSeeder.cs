using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.Entities;
using System.Threading.Tasks;

namespace Server.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(AppDbContext context)
        {
            if (!await context.Roles.AnyAsync(r => r.Name == "classic"))
            {
                context.Roles.Add(new Role { Name = "classic" });
            }

            if (!await context.Roles.AnyAsync(r => r.Name == "seller"))
            {
                context.Roles.Add(new Role { Name = "seller" });
            }

            await context.SaveChangesAsync();
        }
    }
}