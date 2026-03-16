using DocuSense.Application.Dtos;
using DocuSense.Application.Services;
using Microsoft.SemanticKernel.Text;

#pragma warning disable SKEXP0050

namespace DocuSense.Infrastructure.Services
{
    internal class TextChunkerService : ITextChunkerService
    {
        public List<ChunkMetadataDto> ChunkText(List<ChunkMetadataDto> pages, int maxToken)
        {
            var finalChunkedList = new List<ChunkMetadataDto>();
            foreach (var page in pages)
            {
                //boşlukları bölüp tek bir uzun metin haline getiriyoruz.
                string cleanText = page.Text
                .Replace("\r\n", " ")
                .Replace("\n", " ")
                .Replace("\r", " ");

                //SADECE bu sayfanın metnini Semantic Kernel'a verip parçalatıyoruz
                // SK, metni alır ve maxToken sınırına göre List<string> döner
                var stringChunks = TextChunker.SplitPlainTextParagraphs(new List<string> { cleanText }, maxToken);
                foreach (var chunkText in stringChunks)
                {
                    var chunkData = new ChunkMetadataDto()
                    {
                        Text = chunkText, // SK'nın kestiği 500'lük parça
                        PageNumber = page.PageNumber // Dış döngüden gelen o sayfanın numarası
                    };

                    finalChunkedList.Add(chunkData);
                }
            }

            return finalChunkedList;
        }
    }
}