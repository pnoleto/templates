
using Infra.Database.ModelDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infra.RecurringJobs
{
    public class MigrationsJob(NewsDbContext newsDbContext)
    {
        public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
        {
            await newsDbContext.Database.MigrateAsync(cancellationToken);

            return true;
        }
    }
}
