using Application.Results.Base;
using Domain.Models;

namespace Application.Results
{
    public class SourceResult: ResultBase
    {
        public IEnumerable<Source> Sources { get; set; } = [];
    }
}
