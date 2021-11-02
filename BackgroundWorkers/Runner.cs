using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backgroundworker.TaskWorker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backgroundworker
{
    public class Runner : BackgroundService
    {
        private readonly ILogger<Runner> _logger;
        private readonly IBackgroundTaskQueue _queue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Runner(ILogger<Runner> logger, IBackgroundTaskQueue queue, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10_000, stoppingToken);

                _logger.LogError("Runner running at: {time}", DateTimeOffset.Now);
                //AddTask();

                _queue.QueueTask(SampleTask);
            }
        }

        private void AddTask()
        {
            _queue.QueueTask(async token =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine(i);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
            });
        }

        private Task SampleTask(CancellationToken token)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
            }

            return Task.FromResult(true);
        }
    }
}