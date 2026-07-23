using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task_Managment.DAL.Presisitence.Context;
using Task_Managment.PL;

namespace Task_Management.IntegrationTests;

public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove ALL EF Core registrations related to TaskManagemntDbContext
            var descriptorsToRemove = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<TaskManagemntDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    (d.ServiceType.IsGenericType &&
                     d.ServiceType.GetGenericTypeDefinition().Name.Contains("IDbContextOptionsConfiguration")))
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            // Add InMemory Database
            var dbName = $"TaskDb_{Guid.NewGuid()}";

            services.AddDbContext<TaskManagemntDbContext>(options =>
            {
                options.UseInMemoryDatabase(dbName);
            });

            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<TaskManagemntDbContext>();

            db.Database.EnsureDeleted();

            db.Database.EnsureCreated();
        });
    }
}