using Domain.Models;
using Domain.Interfaces.Repositories;
using Infra.Database.Repositories.Base;

namespace Infra.Database.Repositories
{
    public class SourceRepository(ModelDbContext.MainDbContext dbContext) : RepositoryBase<Source>(dbContext), ISourceRepository
    {

    }
}
