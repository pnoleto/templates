namespace Shared
{
    public class JwtSettings
    {
        public string SecretKeyHash { get; set; } = string.Empty;
        public string[] Audiences { get; set; } = [];
        public string[] Issuers { get; set; } = [];
    }
}