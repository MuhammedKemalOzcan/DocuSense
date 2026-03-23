using DocuSense.Application.Dtos;

namespace DocuSense.Application.Services
{
    public interface IChatGenerationService
    {
        IAsyncEnumerable<string> GetChatMessageAsync(List<string> context, string query,List<ChatMessagesListDto> chatHistory, CancellationToken cancellationToken = default);
    }
}