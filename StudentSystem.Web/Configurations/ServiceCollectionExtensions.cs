using SystemTech.Core.JwtManager;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.Web.Common.Helpers;
using StudentSystem.Web.Common.Validators;
using StudentSystem.Web.GraphQl.Students;
using StudentSystem.Web.GraphQl._Type;
using StudentSystem.Web.GraphQl.Auth;
using StudentSystem.Web.GraphQl.Results;
using StudentSystem.Web.GraphQl.Subjects;

namespace StudentSystem.Web.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = appSettings.Jwt.Issuer,
                        ValidAudience = appSettings.Jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Jwt.Key))
                    };
                    
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userid = context.Principal.GetCurrentUserId();
                            var dbContextFactory = context.HttpContext.RequestServices.GetRequiredService<StudentSystemDbContextFactory>();
                            using var dbContext = dbContextFactory.CreateDbContext();
                            var user = dbContext.Users.Find(userid);
                            
                            if (user == null)
                            {
                                context.Fail("Invalid token");
                            }
                            return Task.CompletedTask;
                        },
                    };
                });
            
            services.AddAuthorization();
        }
        
        public static IServiceCollection AddGraphQL(this IServiceCollection services, AppSettings appSettings)
        {
            services
                .AddValidatorsFromAssemblyContaining<AddStudentInputTypeValidator>();

            services
                .AddGraphQLServer()
                .ModifyRequestOptions(o =>
                {
                    o.Complexity.ApplyDefaults = false;
                    o.Complexity.DefaultComplexity = 1;
                    o.Complexity.DefaultResolverComplexity = 5;
                })
                .AddFairyBread()
                .AddAuthorization()
                .RegisterService<AuthService>()
                .RegisterService<StudentService>()
                .RegisterService<SubjectService>()
                .RegisterService<ResultService>()
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .AddInMemorySubscriptions();

            return services;
        }
        
        public static void RegisterServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<AuthService>();
            services.AddTransient<StudentService>();
            services.AddTransient<SubjectService>();
            services.AddTransient<ResultService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IJwtManagerConfiguration, JwtManagerConfiguration>();
            services.AddTransient<IJwtManagerService, JwtManagerService>();
            services.AddScoped<PredicateValidators>();
        }
        
        public static T ConfigureAndGet<T>(
            this IConfiguration configuration, IServiceCollection services) where T: class
        {
            var appSettings = configuration.Get<T>();
            return appSettings;
        }
    }
}