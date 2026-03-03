using DocuSense.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace DocuSense.Persistence.Context
{
    public class DocuSenseAPIDbContext : DbContext
    {
        public DocuSenseAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocuSenseAPIDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}