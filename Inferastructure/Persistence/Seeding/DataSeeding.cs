using Domain_Layer.Contract.Seeding;
using Domain_Layer.Models.Categories_Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.DbContexts;
using System.Text.Json;

namespace Persistence.Seeding
{
    public class DataSeeding(LibraryDbContext _dbContext) : IDataSeeding
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

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
