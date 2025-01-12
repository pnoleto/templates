using Domain.Interfaces.Repositories;
using Domain.Models;
using Infra.Database.ModelDbContext;
using Infra.Database.Repositories.Base;

namespace Infra.Database.Repositories
{
    public class ArticleRepository(NewsDbContext dbContext) : RepositoryBase<Article>(dbContext), IArticleRepository
    {

    }
}
