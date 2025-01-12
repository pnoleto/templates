using Domain.Models.Base;
using Domain.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore.Storage;
using Infra.Database.ModelDbContext;

namespace Infra.Database.Repositories.Base
{
    public class RepositoryBase<T>(NewsDbContext dbContext) : SelectRepositoryBase<T>(dbContext), IRepositoryBase<T> where T : ModelBase
    {
        private IDbContextTransaction? _transaction;
        private readonly NewsDbContext _dbContext = dbContext;

        public T Add(T modelBase)
        {
            var entity = _dbContext.Add(modelBase).Entity;

            _dbContext.SaveChanges();

            return entity;
        }

        public void AddRange(IEnumerable<T> modelBases)
        {
            _dbContext.AddRange(modelBases);

            _dbContext.SaveChanges();
        }

        public T Update(T entity)
        {
            var updatedEntity = _dbContext.Update(entity).Entity;

            _dbContext.SaveChanges();

            return updatedEntity;
        }

        public void UpdateRange(IEnumerable<T> modelBases)
        {
            _dbContext.UpdateRange(modelBases);

            _dbContext.SaveChanges();
        }

        public T Delete(T entity)
        {
            var updatedEntity = _dbContext.Remove(entity).Entity;

            _dbContext.SaveChanges();

            return updatedEntity;
        }

        public void DeleteRange(IEnumerable<T> modelBases)
        {
            _dbContext.RemoveRange(modelBases);

            _dbContext.SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction ??= _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction == null) return;

            _transaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
        }
    }
}
