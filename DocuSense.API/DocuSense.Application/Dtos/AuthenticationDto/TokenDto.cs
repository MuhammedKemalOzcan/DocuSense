namespace DocuSense.Application.Dtos.AuthenticationDto
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}