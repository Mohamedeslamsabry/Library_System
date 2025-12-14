using Domain_Layer.Contract.Seeding;
using Library.Web.Exception_midelWare;

namespace Library.Web.Extension
{
    public static class WebApplictionRegistertion
    {
        public static async Task DataSeedingAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var ObjOfDataSeeding = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjOfDataSeeding.DataSeedAsync();
            await ObjOfDataSeeding.IdentityDataSeedingAsync();
        }

        public static IApplicationBuilder UseCustomeExceptionMidelWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomeExceptionHandlerMidelWare>();
            return app;
        }
    }
}
