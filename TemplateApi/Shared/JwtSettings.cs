namespace Shared
{
    public class JwtSettings: SettingsBase
    {
        public string SecretKeyHash { get; set; } = string.Empty;
        public string[] Audiences { get; set; } = [];
        public string[] Issuers { get; set; } = [];
    }
}