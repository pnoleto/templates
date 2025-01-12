namespace Domain.Interfaces.Repositories.Base
{
    public interface ISelectRepositoryBase<T> where T : class
    {
        IQueryable<T> GetQuerable();
        IEnumerable<T> Get(Func<T, bool> query);
    }
}