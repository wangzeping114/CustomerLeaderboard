using CustomerLeaderboard.Api.Repositories;
using CustomerLeaderboard.Api.Services;
using Microsoft.OpenApi.Models;

namespace CustomerLeaderboard.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Controllers
            services.AddControllers();

            // Add Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer Leaderboard API", Version = "v1" });
                // Enable annotations
                c.EnableAnnotations();
            });

            services.AddEndpointsApiExplorer(); 
            // Add scoped services for dependency injection
            services.AddScoped<ILeaderboardService, LeaderboardService>();
            services.AddScoped<ICustomerRepository, InMemoryCustomerRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve Swagger-UI, specifying the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Leaderboard API V1");
                c.RoutePrefix = string.Empty; // To serve Swagger UI at the app's root (http://localhost:<port>/)
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
