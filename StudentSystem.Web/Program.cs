using AutoMapper;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.Web.GraphQl._Base;
using StudentSystem.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

AppSettings appSettings = builder.Configuration.ConfigureAndGet<AppSettings>(builder.Services);

builder.Services.AddPooledDbContextFactory<StudentSystemDbContext>(option =>
{
    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
    option.UseSqlServer(connection, x => x.EnableRetryOnFailure());
});

builder.Services.AddTransient<StudentSystemDbContextFactory>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddHttpClient();

// Auto mapping
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new GraphQlMapping());
}).CreateMapper());

builder.Services.AddAuthentication(appSettings);

// DI settings
builder.Services.RegisterServices(appSettings);

// GraphQL config
builder.Services.AddGraphQL(appSettings);

builder.Services.AddHealthChecks()
    .AddSqlServer(appSettings.ConnectionStrings.DefaultConnection);

builder.Services.AddErrorFilter<GraphQlErrorFilter>();

var app = builder.Build();

app.UseForwardedHeaders();

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapHealthChecks("/health");
});

app.UseGraphQLVoyager(new VoyagerOptions()
{
    GraphQLEndPoint = "/graphql"
}, "/graphql-voyager");

app.Run();