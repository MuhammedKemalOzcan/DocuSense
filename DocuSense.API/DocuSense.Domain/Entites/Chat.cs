namespace DocuSense.Domain.Entites
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string VectorDocumentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}