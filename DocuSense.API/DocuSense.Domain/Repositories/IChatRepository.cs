using DocuSense.Domain.Entites;

namespace DocuSense.Domain.Repositories
{
    public interface IChatRepository
    {
        Task<Chat> GetByIdAsync(Guid id, string userId);

        void Add(Chat chat);

        void Update(Chat chat);

        void Remove(Chat chat);
    }
}