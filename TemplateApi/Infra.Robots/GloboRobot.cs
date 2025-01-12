using Domain.Models;
using Infra.Robots.Interfaces;

namespace Infra.Robots
{
    public class GloboRobot : IGloboRobot
    { 
        public Task<Source> ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

  
    }
}