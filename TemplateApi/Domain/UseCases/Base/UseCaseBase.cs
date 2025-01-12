using Domain.Interfaces.UseCases.Base;
using Domain.Models.Base;

namespace Domain.UseCases.Base
{
    public abstract class UseCaseBase<TEntity> : IUseCaseBase<TEntity> where TEntity : ModelBase
    {
        public abstract TEntity Execute(TEntity entity);
        public abstract IEnumerable<TEntity> Execute(IEnumerable<TEntity> entity);
    }
}
