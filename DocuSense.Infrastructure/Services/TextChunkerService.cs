using DocuSense.Application.Services;
using Microsoft.SemanticKernel.Text;

#pragma warning disable SKEXP0050

namespace DocuSense.Infrastructure.Services
{
    internal class TextChunkerService : ITextChunkerService
    {
        public List<string> ChunkText(string text, int maxToken)
        {
            List<string> lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            return TextChunker.SplitPlainTextParagraphs(lines, maxToken);
        }
    }
}