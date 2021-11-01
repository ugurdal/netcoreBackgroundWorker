using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backgroundworker.TaskWorker
{
    public class TaskWorkerService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _queue;
        private ILogger<TaskWorkerService> _logger;

        public TaskWorkerService(ILoggerFactory factory, IBackgroundTaskQueue queue)
        {
            _logger = factory.CreateLogger<TaskWorkerService>();
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _queue.DequeueAsync(stoppingToken);
                _logger.LogInformation("Beginning to task {0}", nameof(workItem));

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }
    }
}