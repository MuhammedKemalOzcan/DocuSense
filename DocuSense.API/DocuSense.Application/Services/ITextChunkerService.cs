using DocuSense.Application.Dtos;

namespace DocuSense.Application.Services
{
    public interface ITextChunkerService
    {
        List<ChunkMetadataDto> ChunkText(List<ChunkMetadataDto> pages, int maxToken);
    }
}