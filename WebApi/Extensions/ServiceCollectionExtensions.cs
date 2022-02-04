using DataAccess.DataBase;
using DataAccess.Repositories.Employees;
using DataAccess.Repositories.Reports;
using DataAccess.Repositories.Sprints;
using DataAccess.Repositories.Tasks;
using DataAccess.Repositories.WorkTeams;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services.Services;
using Services.Services.Interfaces;

namespace WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReportsServices(
        this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
    {
        services.AddDbContext<ReportsDbContext>(optionsAction);

        services.AddScoped<IEmployeesRepository, EmployeesRepository>();
        services.AddScoped<IReportTasksRepository, ReportTasksRepository>();
        services.AddScoped<IWorkTeamsRepository, WorkTeamsRepository>();
        services.AddScoped<IReportsRepository, ReportsRepository>();
        services.AddScoped<ISprintsRepository, SprintsRepository>();

        services.AddScoped<IEmployeesService, EmployeesService>();
        services.AddScoped<ITasksService, TasksService>();
        services.AddScoped<IWorkTeamsService, WorkTeamsService>();
        services.AddScoped<IReportsService, ReportsService>();
        services.AddScoped<ISprintsService, SprintsService>();

        return services;
    }

    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.AddAuthorization(opt =>
            opt.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());

        return services;
    }

    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.UseInlineDefinitionsForEnums();
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ASP-Web-API-multi-layer",
                Version = "v1",
                Description =
                    "To try out all the requests you have to be authorized (check the <b>Authorize</b> section)",
            });

            opt.AddSecurityDefinition("Bearer (value: SecretKey)", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer (value: SecretKey)",
                            Type = ReferenceType.SecurityScheme,
                        },
                    },
                    new List<string>()
                },
            });
        });

        return services;
    }
}