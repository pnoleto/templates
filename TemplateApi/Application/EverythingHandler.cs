using Domain.Interfaces.Repositories.Base;
using Domain.Models;
using MediatR;
using Application.DTO.Base;
using FluentValidation;
using Application.DTO;

namespace Application
{
    public class EverythingHandler : BaseValidationHandler<EverythingQuery>, IRequestHandler<EverythingEvent, ArticleResult>
    {
        private readonly ISelectRepositoryBase<Article> selectRepository;

        public EverythingHandler(ISelectRepositoryBase<Article> selectRepository)
        {
            this.selectRepository = selectRepository;
        }

        public Task<ArticleResult> Handle(EverythingEvent request, CancellationToken cancellationToken)
        {
            return Task.Run(async() =>
            {
                await this.ValidateAndThrowAsync(request);

                IQueryable<Article> query = selectRepository.GetQueryable();

                if (!string.IsNullOrEmpty(request.QInTitle)) query = query.Where(x => x.Title.Contains(request.Source));

                if (!string.IsNullOrEmpty(request.Q)) query = query.Where(x => x.Title.Contains(request.Q) || x.Description.Contains(request.Q) || x.Content.Contains(request.Q));

                if (!string.IsNullOrEmpty(request.Source)) query = query.Where(x => x.Source.Name.Contains(request.Source));

                if (!string.IsNullOrEmpty(request.Language)) query = query.Where(x => x.Source.Feeds.Where(y => y.Language == request.Language).Any());

                if (request.From.HasValue) query = query.Where(x => x.PublishedAt >= request.From);

                if (request.To.HasValue) query = query.Where(x => x.PublishedAt >= request.To);

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
