using Application.DTO;
using Application.Results;
using MediatR;

namespace Application.Events
{
    public class SourceEvent : SourceQuery, IRequest<SourceResult>
    {
    }
}
