using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReportsLibrary.Employees;
using ReportsWebApiLayer.DataBase;
using ReportsWebApiLayer.Services;
using ReportsWebApiLayer.Services.Interfaces;

namespace ReportsWebApiLayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ReportsDbContext>(opt =>
                opt.UseSqlite("Data Source=Reports.db;Cache=Shared;"));

            services.AddSwaggerGen(c => 
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebApiReports", Version = "v1"}));

            // уничтожаются после окончания запросов
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ITaskService, TaskService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // var config = new MapperConfiguration(cfg => {
            //     cfg.CreateMap<Employee, Employee>();
            // });

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
}