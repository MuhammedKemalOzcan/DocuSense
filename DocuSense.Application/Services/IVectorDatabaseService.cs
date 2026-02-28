namespace DocuSense.Application.Services
{
    public interface IVectorDatabaseService
    {
        Task IngestDataAsync(List<string> chunks);

        Task<List<string>> SearchDataAsync(string query, int top);
    }
}