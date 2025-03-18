namespace Shared
{
    public class CorsSettings
    {
        public string[] Headers { get; set; } = [];
        public string[] Methods { get; set; } = [];
        public string[] Origins { get; set; } = [];
    }
}