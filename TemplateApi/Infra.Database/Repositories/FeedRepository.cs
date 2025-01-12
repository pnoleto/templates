using Domain.Interfaces.Repositories;
using Domain.Models;
using Infra.Database.ModelDbContext;
using Infra.Database.Repositories.Base;

namespace Infra.Database.Repositories
{
    public class FeedRepository(NewsDbContext dbContext) : RepositoryBase<Feed>(dbContext), IFeedRepository
    {
    }
}
