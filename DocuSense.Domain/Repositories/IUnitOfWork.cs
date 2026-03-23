namespace DocuSense.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IChatRepository ChatRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}