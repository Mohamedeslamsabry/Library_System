using Domain_Layer.Contract.Seeding;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Data.DbContexts;
using Persistence.Data.IdentityContext;
using Persistence.Implment_repo;
using Persistence.Seeding;
using Service_Abstraction.Interfaces;
using Service_Implemention.Mapper;
using Service_Implemention.Service;
using System.Security.Claims;
using System.Text;

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



            #region SWagger Ui
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Library API",
                    Version = "v1"
                });

                // تعريف مخطط الأمان: Bearer JWT في الهيدر Authorization
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",                       // ← لازم يكون Authorization
                    Description = "Add Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,               // ← مش ApiKey
                    Scheme = "bearer",                            // ← لازم lowercase
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                // نجبر كل العمليات إنها تستخدم الـ Bearer تلقائيًا
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
            });

            #endregion


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
            builder.Services.AddScoped<IBorrowService, BorrowingService>();
            builder.Services.AddScoped<IAuthentctionService, AuthentcationService>();

            builder.Services.AddSingleton<IRevokedTokenStore, InMemoryRevokedTokenStore>();


            builder.Services.AddAutoMapper(M => M.AddProfile(new MapingProfile()));


            #region Identity
            builder.Services.AddDbContext<LibraryIdentityContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

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

            #region JWT
            builder.Services.AddAuthentication(Config =>
               {
                   Config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   Config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               }).AddJwtBearer(options =>
               {
                   //options.SaveToken = true;
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidIssuer = builder.Configuration["JWTOptions:issuer"],

                       ValidateAudience = true,
                       ValidAudience = builder.Configuration["JWTOptions:audience"],

                       ValidateLifetime = true,


                       ValidateIssuerSigningKey = true,


                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTOptions")["SecritKey"]!)),


                       // يفضل تقلّل أو تصفّر الانحراف
                       ClockSkew = TimeSpan.Zero,

                       // يضمن وجود Exp في كل التوكينات
                       RequireExpirationTime = true,


                   };


                   options.Events = new JwtBearerEvents
                   {
                       OnTokenValidated = async ctx =>
                       {
                           var userManager = ctx.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

                           var userId = ctx.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                           var ssClaim = ctx.Principal?.FindFirst("ss")?.Value;

                           if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(ssClaim))
                           {
                               ctx.Fail("Token missing required claims.");
                               return;
                           }

                           var user = await userManager.FindByIdAsync(userId);
                           if (user is null || user.SecurityStamp != ssClaim)
                           {

                               // SecurityStamp اتغيّر (مثلاً بعد Reset Password)
                               ctx.Fail("Token invalidated due to security stamp change.");
                           }
                       }
                   };

               });

            #endregion

            var app = builder.Build();

            #region Seeding
            using var Scope = app.Services.CreateScope();
            var ObjOfDataSeeding = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            await ObjOfDataSeeding.DataSeedAsync();
            await ObjOfDataSeeding.IdentityDataSeedingAsync();
            #endregion

            #region  Configure the HTTP request pipeline.
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

            app.Run();
        }
    }
}
