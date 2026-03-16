namespace DocuSense.Application.Dtos
{
    public class ChatMessagesListDto
    {
        public string Text { get; set; }
        public bool IsUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}