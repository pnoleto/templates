using Microsoft.Extensions.DependencyInjection;
using Infra.Robots;
using Quartz;

namespace Infra.DI
{
    public static partial class QuartzSchedulerRegister
    {
        private static ITriggerConfigurator LoadDefaultSettings(this ITriggerConfigurator trigger)
        {
            return trigger.StartNow()
                          .WithIdentity(nameof(Job.Execute))
                          .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
                          .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(30)).RepeatForever())
                          .WithDescription("Feed job responsible for news feed consume");
        }

        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services) => services
            .Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = false;
                options.Scheduling.OverWriteExistingData = true;
            })
            .AddQuartz(options =>
            {
                options.UseSimpleTypeLoader();
                options.UseInMemoryStore();
                options.UseDefaultThreadPool(TTL => TTL.MaxConcurrency = 10);
                options.ScheduleJob<Job>(trigger => trigger.LoadDefaultSettings());
            })
            .AddQuartzHostedService(options => options.WaitForJobsToComplete = true)
            .AddScheduledJobs();

    }
}
