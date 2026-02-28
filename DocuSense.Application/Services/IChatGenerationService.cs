namespace DocuSense.Application.Services
{
    public interface IChatGenerationService
    {
        Task<string> GetChatMessageAsync(List<string> context, string query);
    }
}