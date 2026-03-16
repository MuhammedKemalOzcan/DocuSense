using DocuSense.Application.Dtos;
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

        public async IAsyncEnumerable<string> GetChatMessageAsync(List<string> context, string query, List<ChatMessagesListDto> chatHistory, CancellationToken cancellationToken)
        {
            var joinedContext = string.Join(" ", context);

            var history = new ChatHistory();

            //Sistemin görevini ve RAG bağlamı "System" rolüyle verildi.
            history.AddSystemMessage($@"Sen DocuSense adında kurumsal bir döküman asistanısın.
            Aşağıda sana sağlanan bilgi parçalarını kullanarak kullanıcının sorusunu cevapla.

            BİLGİ PARÇALARI:
            {joinedContext}

            ÖNEMLİ KURALLAR:
            1. Sadece sana verilen bilgi parçalarını kullan.
            2. Cevabını verirken, kullandığın bilginin hangi sayfadan geldiğini cümlenin sonuna MUTLAKA ekle.
            3. Kaynak gösterme formatı şu şekilde olmalıdır: '(Sayfa: X)'");

            //mesajları kullanıcı ve asistan olarak ayırıyoruz.
            foreach (var message in chatHistory)
            {
                if (message.IsUser) history.AddUserMessage(message.Text);
                else history.AddAssistantMessage(message.Text);
            }

            //kullanıcının sorgusunu son olarak gönderiyoruz
            history.AddUserMessage(query);

            var result = _chatCompletionService.GetStreamingChatMessageContentsAsync(history, null, null, cancellationToken);

            await foreach (var item in result)
            {
                if (item.Content != null) yield return item.Content;
            }
        }
    }
}