using Domain_Layer.Models.Identity;
using Library.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace Library.Web.Extension
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddSwigerService(this IServiceCollection Services)
        {

            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen(c =>
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
            return Services;
        }

        public static IServiceCollection AddJWTService(this IServiceCollection Services, IConfiguration _configuration)
        {
            Services.AddAuthentication(Config =>
            {
                Config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWTOptions:issuer"],

                    ValidateAudience = true,
                    ValidAudience =_configuration["JWTOptions:audience"],

                    ValidateLifetime = true,


                    ValidateIssuerSigningKey = true,


                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTOptions")["SecritKey"]!)),


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
            return Services;
        }

        public static IServiceCollection AddWebAppictionService(this IServiceCollection Services)
        {
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.ValidtionErrorResponse;
            });
            return Services;
        }
    }
}
