using Domain_Layer.Contract.Seeding;
using Domain_Layer.Contract.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.DbContexts;
using Persistence.Implment_repo;
using Persistence.Seeding;
using Service_Abstraction.Interfaces;
using Service_Implemention.Mapper;
using Service_Implemention.Service;

namespace Library.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region  Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<LibraryDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DeafultConnection"));
                Options.UseLazyLoadingProxies();
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddScoped<IFloorService, FloorService>();
            builder.Services.AddScoped<IShelfService, ShelfService>();
            builder.Services.AddScoped<IPublisherService, PublisherService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IAuthorsService, AuthorService>();
            builder.Services.AddScoped<ICategoriService, categoryService>();




            builder.Services.AddAutoMapper(M => M.AddProfile(new MapingProfile()));
            #endregion

            var app = builder.Build();

            #region Seeding
            using var Scope = app.Services.CreateScope();
            var ObjOfDataSeeding = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            await ObjOfDataSeeding.DataSeedAsync();
            //await ObjOfDataSeeding.IdentityDataSeedingAsync();
            #endregion

            #region  Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
