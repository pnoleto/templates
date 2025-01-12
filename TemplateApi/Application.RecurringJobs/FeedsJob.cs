using Domain.Interfaces.Repositories.Base;
using Infra.Robots.Interfaces;
using Domain.Models.Base;

namespace Infra.RecurringJobs
{
    public class FeedsJob(
        IGloboRobot globoRobot,
        IFolhaRobot folhaRobot,
        IGazetaRobot gazetaRobot,
        IRepositoryBase<ModelBase> repository)
    {
        public async Task Execute(CancellationToken cancellationToken)
        {
            try
            {
                repository.BeginTransaction();

                await GloboFeed(cancellationToken);

                await FolhaFeeds(cancellationToken);

                await GazetaFeeds(cancellationToken);

                repository.CommitTransaction();
            }
            finally
            {
                repository.RollbackTransaction();
            }
        }

        private async Task GloboFeed(CancellationToken cancellationToken)
        {
            var globoFeeds = await globoRobot.ExecuteAsync(cancellationToken);

            repository.Add(globoFeeds);
        }

        private async Task FolhaFeeds(CancellationToken cancellationToken)
        {
            var folhaFeeds = await folhaRobot.ExecuteAsync(cancellationToken);

            repository.Add(folhaFeeds);
        }

        private async Task GazetaFeeds(CancellationToken cancellationToken)
        {
            var gazetaFeeds = await gazetaRobot.ExecuteAsync(cancellationToken);

            repository.Add(gazetaFeeds);
        }

    }
}
