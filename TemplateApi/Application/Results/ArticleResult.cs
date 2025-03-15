using Application.Results.Base;
using Domain.Models;

namespace Application.Results
{
    public class ArticleResult : ResultBase
    {
        public IEnumerable<Article> Articles { get; set; } = [];
        public int? TotalResults { get; set; }
        public int? TotalPages { get; set; }
    }
}
