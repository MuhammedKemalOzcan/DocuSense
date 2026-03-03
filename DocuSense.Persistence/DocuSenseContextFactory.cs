using DocuSense.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DocuSense.Persistence
{
    public class DocuSenseContextFactory : IDesignTimeDbContextFactory<DocuSenseAPIDbContext>
    {
        public DocuSenseAPIDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DocuSenseAPIDbContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Database=docusense;Username=postgres;Password=123");

            return new DocuSenseAPIDbContext(optionsBuilder.Options);
        }
    }
}