using Domain.Interfaces.Repositories;
using Domain.Models;
using Infra.Database.Repositories.Base;

namespace Infra.Database.Repositories
{
    public class FeedRepository(ModelDbContext.MainDbContext dbContext) : RepositoryBase<Feed>(dbContext), IFeedRepository
    {
    }
}
