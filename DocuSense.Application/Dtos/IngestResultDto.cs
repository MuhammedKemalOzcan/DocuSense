namespace DocuSense.Application.Dtos
{
    public class IngestResultDto
    {
        public string DocumentId { get; set; }
        public Guid ChatId { get; set; }
        public string ChatTitle { get; set; }
    }
}