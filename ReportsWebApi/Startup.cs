using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReportsDataAccess.DataBase;
using ReportsInfrastructure.Services;
using ReportsInfrastructure.Services.Interfaces;
using ReportsWebApi.Middlewares;

namespace ReportsWebApi;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddDbContext<ReportsDbContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("SQLiteDb")));

        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiReports", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
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
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme,
                        },
                    }, new List<string>()
                },
            });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.AddAuthorization(opt =>
            opt.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());

        // removing after completion of requests
        services.AddScoped<IEmployeesService, EmployeesService>();
        services.AddScoped<ITasksService, TasksService>();
        services.AddScoped<IWorkTeamsService, WorkTeamsService>();
        services.AddScoped<IReportsService, ReportsService>();
        services.AddScoped<ISprintsService, SprintsService>();

        services.AddAutoMapper(typeof(Startup));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiReports v1"));

        app.UseHttpsRedirection();
        app.UseRouting();

        if (env.IsDevelopment())
        {
            app.UseMiddleware<CustomAuthorizationMiddleware>();
        }
        else
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        app.UseMiddleware<RequestValidationMiddleware>();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}