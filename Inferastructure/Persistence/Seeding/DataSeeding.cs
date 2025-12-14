using Domain_Layer.Contract.Seeding;
using Domain_Layer.Models.Authors_Models;
using Domain_Layer.Models.Categories_Models;
using Domain_Layer.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.DbContexts;
using Persistence.Data.IdentityContext;
using System.Text.Json;

namespace Persistence.Seeding
{
    public class DataSeeding
        (LibraryDbContext _dbContext,
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager,
        LibraryIdentityContext identityContext
        ) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var PendingMigration = await _dbContext.Database.GetPendingMigrationsAsync();
                if (PendingMigration.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.Categories.Any())
                {
                    
                    var categoriesData = File.OpenRead(@"..\Inferastructure\Persistence\Data\DataSeed\categories.json");

                    var Categories = await JsonSerializer.DeserializeAsync<List<Categories>>(categoriesData);

                    if (Categories != null && Categories.Any())
                    {
                        await _dbContext.AddRangeAsync(Categories);
                    }
                }

                if (!_dbContext.Authors.Any())
                {

                    var authorsData = File.OpenRead(@"..\Inferastructure\Persistence\Data\DataSeed\authors_seed.json");

                    var authors = await JsonSerializer.DeserializeAsync<List<Authors>>(authorsData);

                    if (authors != null && authors.Any())
                    {
                        await _dbContext.AddRangeAsync(authors);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public async Task IdentityDataSeedingAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "Eslam",
                        Email = "Mohamed@gmail.com",
                        PhoneNumber = "0123456789",
                        UserName = "MohamedEslam"
                    };

                    var User02 = new ApplicationUser()
                    {
                        FirstName = "Rawan",
                        LastName = "Yarek",
                        Email = "Rawan@gmail.com",
                        PhoneNumber = "0123456799",
                        UserName = "RawanTarek"
                    };

                    await _userManager.CreateAsync(User01, "P@ssw0rd");
                    await _userManager.CreateAsync(User02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");
                }

                await identityContext.SaveChangesAsync();

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            ;
        }
    }
}
