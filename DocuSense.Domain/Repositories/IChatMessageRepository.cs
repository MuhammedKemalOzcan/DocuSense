using DocuSense.Domain.Entites;

namespace DocuSense.Domain.Repositories
{
    public interface IChatMessageRepository
    {
        void Add(ChatMessage chatMessage);
    }
}