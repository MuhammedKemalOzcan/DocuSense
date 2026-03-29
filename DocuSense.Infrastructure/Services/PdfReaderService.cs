using DocuSense.Application.Dtos;
using DocuSense.Application.Services;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace DocuSense.Infrastructure.Services
{
    public class PdfReaderService : IPdfReaderService
    {
        public List<ChunkMetadataDto> ExtractTextFromPdf(Stream stream)
        {
            //Bu sınıf RAM'de tek bir alan ayırır ve metinleri çok yüksek performansla uca uca ekler.
            //StringBuilder stringBuilder = new StringBuilder();

            List<ChunkMetadataDto> chunks = new List<ChunkMetadataDto>();

            using (PdfDocument document = PdfDocument.Open(stream))
            {
                foreach (var page in document.GetPages())
                {
                    var chunkData = new ChunkMetadataDto()
                    {
                        PageNumber = page.Number,
                        Text = ContentOrderTextExtractor.GetText(page)
                    };
                    chunks.Add(chunkData);
                }
            }

            return chunks;
        }
    }
}