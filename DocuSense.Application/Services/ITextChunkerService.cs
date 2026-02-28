namespace DocuSense.Application.Services
{
    public interface ITextChunkerService
    {
        List<string> ChunkText(string text, int maxToken);
    }
}