using DocuSense.Application.Dtos;

namespace DocuSense.Application.Services
{
    public interface IPdfReaderService
    {
        List<ChunkMetadataDto> ExtractTextFromPdf(Stream stream);
    }
}