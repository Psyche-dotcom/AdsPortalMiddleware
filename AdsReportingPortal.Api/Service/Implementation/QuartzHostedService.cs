using Quartz.Spi;
using Quartz;

namespace AdsReportingPortal.Api.Service.Implementation
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private IScheduler _scheduler;

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;
            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_scheduler != null)
            {
                await _scheduler.Shutdown(cancellationToken);
            }
        }
    }
}
