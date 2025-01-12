namespace Domain.Interfaces.UseCases.Base
{
    public interface IUseCaseBase<TEntity>
    {
        TEntity Execute(TEntity entity);
        IEnumerable<TEntity> Execute(IEnumerable<TEntity> entity);
    }
}
