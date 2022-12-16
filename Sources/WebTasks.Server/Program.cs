
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebTasks.Server.Data;
using WebTasks.Shared;

namespace WebTasks.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = BuildConnectionString(builder.Configuration);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (builder.Configuration.GetValue<bool>("UseLocalSqliteDb"))
                {
                    options.UseSqlite("Data Source=db.sqlite;")
                    .UseLazyLoadingProxies();
                }
                else
                {
                    options.UseNpgsql(connectionString)
                    .UseLazyLoadingProxies();
                }
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(SetupSwagger);
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebTasks.Server V1");
                    c.RoutePrefix = "doc";
                });
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static string BuildConnectionString(IConfiguration configuration)
        {
            NpgsqlConnectionStringBuilder builder = new()
            {
                Username = configuration["PG_Username"],
                Password = configuration["PG_Password"],
                Host = configuration["PG_Server"],
                Database = configuration["PG_Database"],
                Port = configuration.GetValue<int>("PG_Port")
            };
            return builder.ConnectionString;
        }

        private static void SetupSwagger(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "WebTasks.Server",
                Description = "Implementation of a web API for entering project data into a database (task tracker).",
                Contact = new OpenApiContact
                {
                    Name = "Anastasiia B.",
                    Email = "stewie.belova@gmail.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Swagger License",
                    Url = new Uri("https://swagger.io/license/")
                }
            });
            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        }
    }
}