namespace Domain.Interfaces.Repositories.Base
{
    public interface ISelectRepositoryBase<T> where T : class
    {
        IQueryable<T> GetQueryable();
        IEnumerable<T> Get(Func<T, bool> query);
    }
}