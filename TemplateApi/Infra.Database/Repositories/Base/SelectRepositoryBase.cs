using Domain.Interfaces.Repositories.Base;
using Infra.Database.ModelDbContext;
using Domain.Models.Base;

namespace Infra.Database.Repositories.Base
{
    public class SelectRepositoryBase<T>(NewsDbContext dbContext) : ISelectRepositoryBase<T> where T : ModelBase
    {
        private readonly NewsDbContext _dbContext = dbContext;

        public IEnumerable<T> Get(Func<T, bool> query)
        {
            return _dbContext.Set<T>().Where(query);
        }

        public IQueryable<T> GetQuerable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
    }
}
