using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services;
using ReportsDataAccessLayer.Services.Interfaces;

namespace ReportsWebApiLayer;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Startup));

        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddDbContext<ReportsDbContext>(opt =>
            opt.UseSqlite("Data Source=Reports.db;Cache=Shared;"));

        services.AddSwaggerGen(c =>
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiReports", Version = "v1" }));

        // removing after completion of requests
        services.AddScoped<IEmployeesService, EmployeesService>();
        services.AddScoped<ITasksService, TasksService>();
        services.AddScoped<IWorkTeamsService, WorkTeamsesService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiReports v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}