using Domain.Interfaces.Repositories.Base;
using Domain.Models.Base;

namespace Infra.Database.Repositories.Base
{
    public class SelectRepositoryBase<T>(ModelDbContext.MainDbContext dbContext) : ISelectRepositoryBase<T> where T : ModelBase
    {
        private readonly ModelDbContext.MainDbContext _dbContext = dbContext;

        public IEnumerable<T> Get(Func<T, bool> query)
        {
            return _dbContext.Set<T>().Where(query);
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
    }
}
