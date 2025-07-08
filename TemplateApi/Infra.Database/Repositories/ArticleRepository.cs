using Domain.Interfaces.Repositories;
using Domain.Models;
using Infra.Database.Repositories.Base;

namespace Infra.Database.Repositories
{
    public class ArticleRepository(ModelDbContext.MainDbContext dbContext) : RepositoryBase<Article>(dbContext), IArticleRepository
    {

    }
}
