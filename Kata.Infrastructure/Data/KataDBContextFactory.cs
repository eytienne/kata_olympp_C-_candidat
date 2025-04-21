using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration.Json;


namespace Kata.Infrastructure.Data
{
    public class KataDBContextFactory : IDesignTimeDbContextFactory<KataDBContext>
    {
        public KataDBContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetParent(Directory.GetCurrentDirectory()) + "/Kata.API";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<KataDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new KataDBContext(optionsBuilder.Options);
        }
    }
}