using Microsoft.EntityFrameworkCore;
using WebApi.Extensions;
using WebApi.Middlewares;

namespace WebApi;

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

        services.AddSwaggerServices();
        services.AddAuthServices();
        services.AddReportsServices(builder =>
        {
            builder.EnableSensitiveDataLogging();
            builder.UseSqlite(Configuration.GetConnectionString("Database"));
        });

        services.AddAutoMapper(typeof(Startup));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            if (!env.IsDevelopment())
                c.RoutePrefix = string.Empty;

            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP-Web-API-multi-layer v1");
        });

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseMiddleware<CustomAuthorizationMiddleware>();
        app.UseMiddleware<CustomCachingMiddleware>();
        app.UseMiddleware<RequestValidationMiddleware>();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}