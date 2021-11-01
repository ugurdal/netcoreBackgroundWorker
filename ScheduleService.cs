using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backgroundworker
{
    public class ScheduleService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private int _counter = 1;

        public ScheduleService(ILogger<ScheduleService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (_counter > 20)
                return;
            //_logger.LogWarning("{counter} work triggered at: {time}", _counter, DateTimeOffset.Now);
            _ = DoWorkAsync(_counter);
            _counter++;
        }

        private async Task DoWorkAsync(int id)
        {
            _logger.LogInformation("{counter} START: {time}"
                , id.ToString().PadLeft(2, '0'), DateTimeOffset.Now);

            await Task.Delay(7_500);

            _logger.LogWarning("{counter}  DONE: {time}",
                id.ToString().PadLeft(2, '0'), DateTimeOffset.Now);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}