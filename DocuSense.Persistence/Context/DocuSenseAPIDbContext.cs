using DocuSense.Application.Data;
using DocuSense.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocuSense.Persistence.Context
{
    public class DocuSenseAPIDbContext : IdentityDbContext<IdentityUser>, IDocuSenseAPIDbContext
    {
        public DocuSenseAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocuSenseAPIDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}