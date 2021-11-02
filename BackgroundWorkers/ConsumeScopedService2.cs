using backgroundworker.ScopedService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace backgroundworker
{
    public class ConsumeScopedService2 : BackgroundService
    {
        private ILogger<ConsumeScopedService2> _logger;

        public ConsumeScopedService2(
            IServiceProvider services,
            ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<ConsumeScopedService2>();
            Services = services;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
            "Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using var scope = Services.CreateScope();

            var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopeSettingsService>();
            await scopedProcessingService.WriteSetting(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
