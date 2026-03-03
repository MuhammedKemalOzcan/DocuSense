using DocuSense.Domain.Entites;
using DocuSense.Domain.Repositories;
using DocuSense.Persistence.Context;

namespace DocuSense.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly DocuSenseAPIDbContext _context;

        public ChatRepository(DocuSenseAPIDbContext context)
        {
            _context = context;
        }

        public void Add(Chat chat)
        {
            _context.Chats.Add(chat);
        }
    }
}