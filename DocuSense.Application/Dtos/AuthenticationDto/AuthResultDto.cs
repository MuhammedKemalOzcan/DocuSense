namespace DocuSense.Application.Dtos.AuthenticationDto
{
    public class AuthResultDto
    {
        public string UserId { get; set; }
        public bool Succeed { get; set; }
        public string? AccessToken { get; set; }
        public string? Error { get; set; }
        public DateTime Expiration { get; set; }
    }
}