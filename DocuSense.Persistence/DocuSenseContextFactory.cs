using DocuSense.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DocuSense.Persistence
{
    public class DocuSenseContextFactory : IDesignTimeDbContextFactory<DocuSenseAPIDbContext>
    {
        public DocuSenseAPIDbContext CreateDbContext(string[] args)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "../DocuSense.API/");

            ConfigurationBuilder configurationManager = new ConfigurationBuilder();

            IConfiguration configuration = configurationManager.SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

            var connectionString = configuration.GetConnectionString("PostgresDb");

            var optionsBuilder = new DbContextOptionsBuilder<DocuSenseAPIDbContext>();

            optionsBuilder.UseNpgsql(connectionString);

            return new DocuSenseAPIDbContext(optionsBuilder.Options);
        }
    }
}