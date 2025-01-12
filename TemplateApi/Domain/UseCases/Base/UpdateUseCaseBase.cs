using Domain.Interfaces.Repositories.Base;
using Domain.Interfaces.UseCases.Base;
using Domain.Models.Base;

namespace Domain.UseCases.Base
{
    public class UpdateUseCaseBase<TEntity>(IRepositoryBase<TEntity> repositoryBase) : UseCaseBase<TEntity>, IUseCaseBase<TEntity> where TEntity : ModelBase
    {
        public override TEntity Execute(TEntity entity)
        {
            TEntity deletedEntity = repositoryBase.Update(entity);

            return deletedEntity;
        }

        public override IEnumerable<TEntity> Execute(IEnumerable<TEntity> entities)
        {
            repositoryBase.UpdateRange(entities);

            return entities;
        }
    }
}
