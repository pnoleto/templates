using Application.DTO;
using Application.DTO.Base;
using MediatR;

namespace Application
{
    public class SourceEvent : SourceQuery, IRequest<SourceResult>
    {
    }
}
