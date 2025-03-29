using Domain.Interfaces.Repositories.Base;
using Domain.Models;
using MediatR;
using FluentValidation;
using Application.Results;
using Application.Events;
using Application.Handlers.Base;

namespace Application.Handlers
{
    public class TopHeadlinesHandler(ISelectRepositoryBase<Article> selectRepository) : 
        HandlerBase<TopHeadlinesEvent>, 
        IRequestHandler<TopHeadlinesEvent, ArticleResult>
    {
        public Task<ArticleResult> Handle(TopHeadlinesEvent request, CancellationToken cancellationToken)
        {
            return Task.Run(async() =>
            {

                await this.ValidateAndThrowAsync(request);

                IQueryable<Article> query = selectRepository
                .GetQueryable();

                if (!string.IsNullOrEmpty(request.Q)) query = query.Where(x => x.Title.Contains(request.Q) || x.Description.Contains(request.Q) || x.Content.Contains(request.Q));

                if (!string.IsNullOrEmpty(request.Category)) query = query.Where(x => x.Source.Feeds.Any(y => string.Equals(y.Category, request.Category)));

                if (!string.IsNullOrEmpty(request.Country)) query = query.Where(x => x.Source.Feeds.Any(y => string.Equals(y.Country, request.Country)));

                if (!string.IsNullOrEmpty(request.Source)) query = query.Where(x => x.Source.Name.Contains(request.Source));

                query = query.Where(x => x.Source.Name.Contains(request.Source)).OrderByDescending(x => x.PublishedAt);

                return new ArticleResult
                {
                    Articles = query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .AsEnumerable(),
                    TotalPages = (int)Math.Ceiling(query.Count() / (decimal)request.PageSize)
                };
            }, cancellationToken);
        }
    }
}
