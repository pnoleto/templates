namespace Domain.Interfaces.Repositories.Base
{
    public interface IRepositoryBase<T> where T : class
    {
        T Add(T entity);
        void AddRange(IEnumerable<T> entities);
        T Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        T Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
