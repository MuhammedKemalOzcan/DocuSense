using DocuSense.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DocuSense.Application.Data
{
    public interface IDocuSenseAPIDbContext
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessage { get; set; }
        DatabaseFacade Database { get; }
    }
}