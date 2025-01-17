using MusicStreamDemo.API.Data;
using MusicStreamDemo.API.Interface;
using MusicStreamDemo.API.Services;

namespace MusicStreamDemo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Azure Blob Storage Service
            builder.Services.AddSingleton<IAzureBlobStorageService>(new AzureBlobStorageService(builder.Configuration.GetConnectionString("AzureBlobStorage")));
            builder.Services.AddSingleton<IDataContext>(new DataContext(builder.Configuration.GetConnectionString("Postgres")));

            // Add controllers
            builder.Services.AddControllers();

            // Add Swagger/OpenAPI services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseCors();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MusicStreamDemo API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            app.Run();
        }
    }
}
