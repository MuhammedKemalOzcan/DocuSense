using DocuSense.Domain.Entites;
using DocuSense.Domain.Repositories;
using DocuSense.Persistence.Context;

namespace DocuSense.Persistence.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly DocuSenseAPIDbContext _context;

        public ChatMessageRepository(DocuSenseAPIDbContext context)
        {
            _context = context;
        }

        public void Add(ChatMessage chatMessage)
        {
            _context.Add(chatMessage);
        }
    }
}