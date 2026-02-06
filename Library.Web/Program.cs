using Domain_Layer.Models.Identity;
using Library.Web.Extension;
using Microsoft.AspNetCore.Identity;
using Persistence.Data.IdentityContext;
using Persistence.Register_Service;
using Service_Abstraction.Interfaces;
using Service_Implemention.Register_Service;
using Service_Implemention.Service;

namespace Library.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region  Add services to the container.

            #region Add automaticly
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
            #endregion

            #region Add Swiger Service
            builder.Services.AddSwigerService();
            #endregion

            #region Presitance (Register Service) 
            builder.Services.AddInferstructureService(builder.Configuration);
            #endregion

            #region Service Implement (Register)
            builder.Services.AddApplictionService();
            #endregion

            #region Identity

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //   .AddEntityFrameworkStores<LibraryIdentityContext>();


            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<LibraryIdentityContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

            #endregion

            #endregion

            #region Model State (Validtion)
            builder.Services.AddWebAppictionService();
            #endregion

            #region AddJWTService
            builder.Services.AddJWTService(builder.Configuration);
            #endregion

            var app = builder.Build();

            #region DataSeeding
            await app.DataSeedingAsync();
            #endregion

            #region  Configure the HTTP request pipeline.

            #region Custome Midel ware
            app.UseCustomeExceptionMidelWare();
            #endregion

            #region pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            #endregion

            app.Run();
        }
    }
}
