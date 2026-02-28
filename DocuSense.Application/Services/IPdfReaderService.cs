namespace DocuSense.Application.Services
{
    public interface IPdfReaderService
    {
        string ExtractTextFromPdf(string filePath);
    }
}