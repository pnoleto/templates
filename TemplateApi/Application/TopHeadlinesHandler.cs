using Domain.Interfaces.Repositories.Base;
using Domain.Models;
using MediatR;
using Application.DTO.Base;

namespace Application
{
    public class TopHeadlinesHandler(ISelectRepositoryBase<Article> selectRepository) : IRequestHandler<TopHeadlinesEvent, ArticleResult>
    {
        public Task<ArticleResult> Handle(TopHeadlinesEvent request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                IQueryable<Article> query = selectRepository
                .GetQuerable();

                if (!string.IsNullOrEmpty(request.Q)) query = query.Where(x => x.Title.Contains(request.Q) || x.Description.Contains(request.Q) || x.Content.Contains(request.Q));

                if (!string.IsNullOrEmpty(request.Category)) query = query.Where(x => x.Source.Feeds.Where(y=> y.Category == request.Category.ToLower()).Any());

                if (!string.IsNullOrEmpty(request.Country)) query = query.Where(x => x.Source.Feeds.Where(y => y.Country == request.Country.ToLower()).Any());

                if (!string.IsNullOrEmpty(request.Source)) query = query.Where(x => x.Source.Name.Contains(request.Source));

                query = query.Where(x => x.Source.Name.Contains(request.Source)).OrderByDescending(x=> x.PublishedAt);

                return new ArticleResult
                {
                    Articles = query
                    .Skip((int)((request.Page - 1) * request.PageSize))
                    .Take(request.PageSize)
                    .AsEnumerable(),
                    TotalPages = (int)Math.Ceiling(query.Count() / (decimal)request.PageSize)
                };
            }, cancellationToken);
        }
    }
}
