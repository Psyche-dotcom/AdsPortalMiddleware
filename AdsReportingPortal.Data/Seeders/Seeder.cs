

using AdsReportingPortal.Data.Context;
using AdsReportingPortal.Model.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace AdsReportingPortal.Data.Seeders
{
    public class Seeder
    {
        public static async Task SeedData(IApplicationBuilder app)
        {

            // Get db context
            var dbContext = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ProjectDbContext>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }

            if (!dbContext.Roles.Any())
            {
                await dbContext.Database.EnsureCreatedAsync();
                
                var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Creating list of roles
                List<string> roles = new() { "Super Admin", "Companies", "User" };

                // Creating roles
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }


            }


            // Saving everything into the database
            await dbContext.SaveChangesAsync();
        }

       
    }
}
