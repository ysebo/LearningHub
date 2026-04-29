using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LearningHub.Infrastructure.Persistence;

public sealed class LearningHubDbContextFactory : IDesignTimeDbContextFactory<LearningHubDbContext>
{
    public LearningHubDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../LearningHub.API");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<LearningHubDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new LearningHubDbContext(optionsBuilder.Options);
    }
}