
using Application.DTO.Base;

namespace Application.DTO
{
    public class SourceQuery : QueryBase
    {
        public string? Category { get; set; }
        public string? Language { get; set; }
        public string? Country { get; set; }
    }
}
