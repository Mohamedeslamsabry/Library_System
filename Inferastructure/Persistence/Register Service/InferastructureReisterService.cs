using Domain_Layer.Contract.Cash;
using Domain_Layer.Contract.Seeding;
using Domain_Layer.Contract.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.DbContexts;
using Persistence.Data.IdentityContext;
using Persistence.Implment_repo;
using Persistence.Seeding;
using StackExchange.Redis;

namespace Persistence.Register_Service
{
    public static class InferastructureReisterService
    {
        public static IServiceCollection AddInferstructureService(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddDbContext<LibraryDbContext>(Options =>
            {
                Options.UseSqlServer(Configuration.GetConnectionString("DeafultConnection"));
                Options.UseLazyLoadingProxies();
            });
            Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Services.AddScoped<IDataSeeding, DataSeeding>();

            #region ConnectionMultiplexer
            Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RediusConnection")!);
            });

            Services.AddScoped<ICashRepo, CashRepositary>();

            #endregion

            Services.AddDbContext<LibraryIdentityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });


            return Services;
        }
    }
}
