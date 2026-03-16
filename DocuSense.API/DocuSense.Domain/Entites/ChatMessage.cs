namespace DocuSense.Domain.Entites
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string Text { get; set; }
        public bool IsUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}