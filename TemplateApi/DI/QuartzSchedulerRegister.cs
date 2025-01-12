using Infra.Schedules;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infra.DI
{
    public static class QuartzSchedulerRegister
    {
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services) => services
            .Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = true; // default: false
                options.Scheduling.OverWriteExistingData = true; // default: true
            })
            .AddQuartz(options =>
            {
                options.UseSimpleTypeLoader();
                options.UseInMemoryStore();
                options.UseDefaultThreadPool(TTL =>
                {
                    TTL.MaxConcurrency = 10;
                });
                options.ScheduleJob<FeedsJob>(trigger =>
                trigger.StartNow()
                  .WithIdentity(nameof(FeedsJob))
                  .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
                  .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(30)).RepeatForever())
                  .WithDescription("Feed job responsible for news feed consume"));
            })
            .AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            })
            .AddScheduledJobs();
    }
}
