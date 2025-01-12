using Infra.Robots.Interfaces;
using Domain.Models;

namespace Infra.Robots
{
    public class FolhaRobot : IFolhaRobot
    {
        public Task<Source> ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}