using DocuSense.Application.Services;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DocuSense.Infrastructure.Services
{
    internal class ChatGenerationService : IChatGenerationService
    {
        private readonly IChatCompletionService _chatCompletionService;

        public ChatGenerationService(IChatCompletionService chatCompletionService)
        {
            _chatCompletionService = chatCompletionService;
        }

        public async Task<string> GetChatMessageAsync(List<string> context, string query)
        {

            var joinedContext = string.Join(" ",context);

            string prompt = $"Sen DocuSense asistanısın. Sadece sana verilen şu metinleri kullanarak cevap ver Bilgiler: {joinedContext}. Soru: {query}";

            var result = await _chatCompletionService.GetChatMessageContentAsync(prompt);

            return result.ToString();
        }
    }
}