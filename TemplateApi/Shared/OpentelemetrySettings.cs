namespace Shared
{
    public class OpentelemetrySettings: SettingsBase
    {
        public string Environment { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceVersion { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
    }
}