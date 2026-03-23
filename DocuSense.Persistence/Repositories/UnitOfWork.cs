using DocuSense.Domain.Repositories;
using DocuSense.Persistence.Context;

namespace DocuSense.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DocuSenseAPIDbContext _context;
        public IChatRepository ChatRepository { get; }

        public UnitOfWork(DocuSenseAPIDbContext context, IChatRepository chatRepository)
        {
            _context = context;
            ChatRepository = chatRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}