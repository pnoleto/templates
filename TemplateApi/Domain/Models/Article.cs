using Domain.Models.Base;

namespace Domain.Models
{
    public sealed class Article : ModelBase
    {
        public Article() { }
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string UrlToImage { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; } = DateTime.MinValue;
        public string Content { get; set; } = string.Empty;
        public long IdSource { get; set; } = 0;
        public Source Source { get; set; } = new Source();
    }
}
