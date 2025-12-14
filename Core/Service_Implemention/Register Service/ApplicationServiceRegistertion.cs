using Microsoft.Extensions.DependencyInjection;
using Service_Abstraction.Interfaces;
using Service_Implemention.Mapper;
using Service_Implemention.Service;

namespace Service_Implemention.Register_Service
{
    public static class ApplicationServiceRegistertion
    {
        public static IServiceCollection AddApplictionService(this IServiceCollection Services)
        {
            Services.AddScoped<IEmployeeService, EmployeeService>();

            Services.AddScoped<IFloorService, FloorService>();
            Services.AddScoped<IShelfService, ShelfService>();
            Services.AddScoped<IPublisherService, PublisherService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IBookService, BookService>();
            Services.AddScoped<IAuthorsService, AuthorService>();
            Services.AddScoped<ICategoriService, categoryService>();
            Services.AddScoped<IBorrowService, BorrowingService>();
            Services.AddScoped<IAuthentctionService, AuthentcationService>();
            Services.AddScoped<ICashService, CashService>();
            Services.AddAutoMapper(M => M.AddProfile(new MapingProfile()));
            Services.AddSingleton<IRevokedTokenStore, InMemoryRevokedTokenStore>();
            return Services;
        }
    }
}
