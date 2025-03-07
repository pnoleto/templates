﻿using Domain.Interfaces.Repositories.Base;
using Domain.Models;
using MediatR;
using Application.DTO.Base;
using FluentValidation;

namespace Application
{
    public class TopHeadlinesHandler : BaseValidationHandler<TopHeadlinesEvent>, IRequestHandler<TopHeadlinesEvent, ArticleResult>
    {
        private readonly ISelectRepositoryBase<Article> selectRepository;

        public TopHeadlinesHandler(ISelectRepositoryBase<Article> selectRepository)
        {
            this.selectRepository = selectRepository;
        }

        public Task<ArticleResult> Handle(TopHeadlinesEvent request, CancellationToken cancellationToken)
        {
            return Task.Run(async() =>
            {

                await this.ValidateAndThrowAsync(request);

                IQueryable<Article> query = selectRepository
                .GetQueryable();

                if (!string.IsNullOrEmpty(request.Q)) query = query.Where(x => x.Title.Contains(request.Q) || x.Description.Contains(request.Q) || x.Content.Contains(request.Q));

                if (!string.IsNullOrEmpty(request.Category)) query = query.Where(x => x.Source.Feeds.Where(y=> string.Equals(y.Category, request.Category)).Any());

                if (!string.IsNullOrEmpty(request.Country)) query = query.Where(x => x.Source.Feeds.Where(y => string.Equals(y.Country, request.Country)).Any());

                if (!string.IsNullOrEmpty(request.Source)) query = query.Where(x => x.Source.Name.Contains(request.Source));

                query = query.Where(x => x.Source.Name.Contains(request.Source)).OrderByDescending(x=> x.PublishedAt);

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
