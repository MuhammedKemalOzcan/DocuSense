using DocuSense.Application.Services;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace DocuSense.Infrastructure.Services
{
    public class PdfReaderService : IPdfReaderService
    {
        public string ExtractTextFromPdf(string filePath)
        {
            //Bu sınıf RAM'de tek bir alan ayırır ve metinleri çok yüksek performansla uca uca ekler.
            StringBuilder stringBuilder = new StringBuilder();
            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    stringBuilder.Append(ContentOrderTextExtractor.GetText(page));
                }
            }

            return stringBuilder.ToString();
        }
    }
}