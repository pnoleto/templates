using Domain.Interfaces.Repositories.Base;
using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface ISourceRepository : ISelectRepositoryBase<Source>, IRepositoryBase<Source>
    {
    }
}
