using Application.DTO;
using Application.Results;
using MediatR;

namespace Application.Events
{
    public class TopHeadlinesEvent : TopHeadlinesQuery, IRequest<ArticleResult>
    {
    }
}
