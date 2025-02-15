using Infra.Robots;
using Infra.Robots.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public static class RobotsRegister
    {
        public static IServiceCollection AddFeedRobots(this IServiceCollection services) => services
            .AddTransient<IFolhaRobot,FolhaRobot>()
            .AddTransient<IGloboRobot,GloboRobot>()
            .AddTransient<IGazetaRobot,GazetaRobot>();
    }
}
