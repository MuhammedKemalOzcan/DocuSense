namespace DocuSense.Application.Dtos
{
    public class IngestResultDto
    {
        public string DocumentId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}