namespace DeltaX.DemoServerTodoAppV1
{
    using DeltaX.DemoServerTodoAppV1.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Data.Sqlite;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
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
            services.AddTransient<IDbConnection, SqliteConnection>(s =>
            {
                var db = new SqliteConnection(connectionString);
                db.Open();
                return db;
            });

            DapperSqliteTypeHandler.SetSqliteTypeHandler();
            services.AddSingleton<ITodoRepository, TodoSqliteRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeltaX.DemoServerTodoAppV1", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CreateTable(app, env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeltaX.DemoServerTodoAppV1"));
            }

            // app.UseHttpsRedirection();

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
