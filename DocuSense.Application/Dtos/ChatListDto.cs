namespace DocuSense.Application.Dtos
{
    public class ChatListDto
    {
        public Guid Id { get; set; }
        public string DocumentId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}