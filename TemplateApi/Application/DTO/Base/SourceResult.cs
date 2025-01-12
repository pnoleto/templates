using Domain.Models;

namespace Application.DTO.Base
{
    public class SourceResult: ResultBase
    {
        public IEnumerable<Source> Sources { get; set; } = [];
    }
}
