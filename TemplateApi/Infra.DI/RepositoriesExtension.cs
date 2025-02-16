using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Base;
using Infra.Database.Repositories;
using Infra.Database.Repositories.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public static class RepositoriesExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddTransient(typeof(ISelectRepositoryBase<>), typeof(SelectRepositoryBase<>))
            .AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
            .AddTransient<IArticleRepository, ArticleRepository>()
            .AddTransient<ISourceRepository, SourceRepository>()
            .AddTransient<IFeedRepository, FeedRepository>();
    }
}
