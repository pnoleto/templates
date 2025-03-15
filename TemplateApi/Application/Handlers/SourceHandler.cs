using Domain.Interfaces.Repositories.Base;
using Domain.Models;
using MediatR;
using FluentValidation;
using Application.Results;
using Application.Events;
using Application.Handlers.Base;

namespace Application.Handlers
{
    public class SourceHandler : BaseValidationHandler<SourceEvent>, IRequestHandler<SourceEvent, SourceResult>
    {
        private readonly ISelectRepositoryBase<Source> selectRepository;

        public SourceHandler(ISelectRepositoryBase<Source> selectRepository)
        {
            this.selectRepository = selectRepository;
        }

        public Task<SourceResult> Handle(SourceEvent request, CancellationToken cancellationToken)
        {
            return Task.Run(async() =>
            {
                await this.ValidateAndThrowAsync(request);

                IQueryable<Source> query = selectRepository
                .GetQueryable();

                if (!string.IsNullOrEmpty(request.Source)) query = query.Where(x => x.Name == request.Source);

                if (!string.IsNullOrEmpty(request.Language)) query = query.Where(x => x.Feeds.Any(y=> y.Language == request.Language));

                if (!string.IsNullOrEmpty(request.Country)) query = query.Where(x => x.Feeds.Any(y => y.Country == request.Country));

                if (!string.IsNullOrEmpty(request.Category)) query = query.Where(x => x.Feeds.Any(y => y.Country == request.Category));

                if (!string.IsNullOrEmpty(request.Q)) query = query
                    .Where(x => x.Feeds.Any(y => 
                    y.Description.StartsWith(request.Q) 
                    || y.Name.StartsWith(request.Q)
                    || y.Country.StartsWith(request.Q)
                    || y.Category.StartsWith(request.Q)
                    || y.Language.StartsWith(request.Q)
                    ));

                return new SourceResult
                {
                    Sources = query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .AsEnumerable(),
                };
            }, cancellationToken);
        }
    }
}

