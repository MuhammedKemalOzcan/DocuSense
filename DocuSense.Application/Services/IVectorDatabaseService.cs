namespace DocuSense.Application.Services
{
    public interface IVectorDatabaseService
    {
        Task IngestDataAsync(List<string> chunks, string documentId);

        Task<List<string>> SearchDataAsync(string query, int top, string documentId);
    }
}