namespace ReportsWebApi;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReportsDataAccess.DataBase;
using ReportsInfrastructure.Services;
using ReportsInfrastructure.Services.Interfaces;
using Middlewares;

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

        services.AddResponseCaching();

        services.AddDbContext<ReportsDbContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("Database")));

        services.AddSwaggerGen(opt =>
        {
            opt.UseInlineDefinitionsForEnums();
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "WebApiReports",
                Version = "v1",
                Description = "To try out all the requests you have to be authorized (check the <b>Authorize</b> section)",
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
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiReports v1");
            });

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseResponseCaching();

        if (env.IsDevelopment())
        {
            app.UseMiddleware<CustomAuthorizationMiddleware>();
        }
        else
        {
            // app.UseAuthentication();
            // app.UseAuthorization();
            app.UseMiddleware<CustomAuthorizationMiddleware>();
        }

        app.UseMiddleware<CustomAuthorizationMiddleware>();
        app.UseMiddleware<RequestValidationMiddleware>();
        app.UseMiddleware<CustomCachingMiddleware>();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}