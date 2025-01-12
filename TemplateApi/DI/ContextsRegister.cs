using Infra.Database.ModelDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public static class ContextsRegister
    {
        private const int THIRTY_SECONDS = 30;
        private const int THREE_RETRIES = 3;

        public static IServiceCollection AddSqlServerDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<NewsDbContext>(cfg => cfg.UseSqlServer(configuration.GetConnectionString("NewsConnection"),
                props =>
                    props.CommandTimeout(THIRTY_SECONDS)
                    .EnableRetryOnFailure(THREE_RETRIES, TimeSpan.FromSeconds(THIRTY_SECONDS), null)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

        public static IServiceCollection AddPostgresDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<NewsDbContext>(cfg => cfg.UseNpgsql(configuration.GetConnectionString("NewsConnection"),
                props =>
                    props.CommandTimeout(THIRTY_SECONDS)
                    .EnableRetryOnFailure(THREE_RETRIES, TimeSpan.FromSeconds(THIRTY_SECONDS), null)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

        public static IServiceCollection AddInMemoryDbContext(this IServiceCollection services) =>
            services.AddDbContext<NewsDbContext>(cfg => cfg.UseInMemoryDatabase("default"));
    }
}
