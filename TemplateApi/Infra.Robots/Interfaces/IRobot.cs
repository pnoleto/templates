using Domain.Models;

namespace Infra.NewsRobot.Interfaces
{
    public interface IRobot
    {
        Task<Source> ExecuteAsync(CancellationToken cancellationToken);
    }
}
