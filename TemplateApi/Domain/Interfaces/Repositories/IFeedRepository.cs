using Domain.Interfaces.Repositories.Base;
using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IFeedRepository : ISelectRepositoryBase<Feed>, IRepositoryBase<Feed>
    {
    }
}
