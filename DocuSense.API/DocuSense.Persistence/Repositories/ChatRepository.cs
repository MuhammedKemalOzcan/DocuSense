using DocuSense.Domain.Entites;
using DocuSense.Domain.Repositories;
using DocuSense.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DocuSense.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly DocuSenseAPIDbContext _context;

        public ChatRepository(DocuSenseAPIDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> GetByIdAsync(Guid id, string userId)
        {
            return await _context.Chats
                .Where(c => c.UserId == userId)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Add(Chat chat)
        {
            _context.Chats.Add(chat);
        }

        public void Remove(Chat chat)
        {
            _context.Chats.Remove(chat);
        }

        public void Update(Chat chat)
        {
            _context.Chats.Update(chat);
        }
    }
}