namespace DeltaX.DemoServerTodoAppV1
{ 
    using DeltaX.DemoServerTodoAppV1.Repositories;
    using DeltaX.DemoServerTodoAppV1.Repositories.Sqlite; 
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Data.Sqlite;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using System;
    using System.Data;


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
            var connectionString = Configuration.GetConnectionString("DemoServerTodo");
            
            DapperSqliteTypeHandler.SetSqliteTypeHandler();

            services.AddSingleton<ITodoCache, TodoCache>(services =>
            {
                var logger = services.GetService<ILogger<TodoCache>>();
                return new TodoCache(logger, TimeSpan.FromHours(5), 1000);
            });

            services.AddTransient<IDbConnection, SqliteConnection>(s =>
            {
                var db = new SqliteConnection(connectionString);
                db.Open();
                return db;
            });          
            services.AddTransient<ITodoRepository, TodoCacheRepository>(); 

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeltaX.DemoServerTodoAppV1", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultVueCors", builder =>
                {
                    builder
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .WithOrigins("http://127.0.0.1:8080", "https://127.0.0.1:8081", "http://localhost:8080");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CreateTable(app, env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            if (env.IsDevelopment() || Configuration.GetValue<bool>("UseSwagger"))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeltaX.DemoServerTodoAppV1"));
            }

            // app.UseHttpsRedirection();

            app.UseCors("DefaultVueCors");

            // serve wwwroot
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void CreateTable(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Create todo schema
            var connection = app.ApplicationServices.GetService<IDbConnection>(); 
            var logger = app.ApplicationServices.GetService<ILogger>(); 
            SqlQueries.CreateDatabase(connection, logger);
        }
    }
}
