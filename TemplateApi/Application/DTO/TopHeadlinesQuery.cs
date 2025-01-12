using Application.DTO.Base;

namespace Application.DTO
{
    public class TopHeadlinesQuery : QueryBase
    {
        public TopHeadlinesQuery() { }
        public string? Category { get; set; }
        public string? Country { get; set; }
    }
}
