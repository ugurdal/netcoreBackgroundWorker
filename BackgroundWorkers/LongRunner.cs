using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backgroundworker
{
    public class LongRunner : BackgroundService
    {
        private readonly ILogger<LongRunner> _logger;

        public LongRunner(ILogger<LongRunner> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning("LongRunner running at: {time}", DateTimeOffset.Now);
                await Task.Delay(2_000, stoppingToken);
            }
        }
    }
}