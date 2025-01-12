using Domain.Models;
using Domain.Interfaces.Repositories;
using Infra.Database.ModelDbContext;
using Infra.Database.Repositories.Base;

namespace Infra.Database.Repositories
{
    public class SourceRepository(NewsDbContext dbContext) : RepositoryBase<Source>(dbContext), ISourceRepository
    {

    }
}
