using Domain.Models.Base;

namespace Domain.Models
{
    public sealed class Source : ModelBase
    {
        public Source() { }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public ICollection<Article> Articles { get; set; } = [];
        public ICollection<Feed> Feeds { get; set; } = [];
        public string SKU { get; set; }
    }
}
