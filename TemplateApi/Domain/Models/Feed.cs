using Domain.Models.Base;

namespace Domain.Models
{
    public sealed class Feed : ModelBase
    {
        public Feed() { }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty ;
        public string Category { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public long IdSource { get; set; } = 0;
        public Source Source { get; set; } = new Source();
    }
}
