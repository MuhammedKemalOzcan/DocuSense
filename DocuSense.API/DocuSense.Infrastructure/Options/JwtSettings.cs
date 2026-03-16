namespace DocuSense.Infrastructure.Options
{
    public sealed class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresMinutes { get; set; }
    }
}