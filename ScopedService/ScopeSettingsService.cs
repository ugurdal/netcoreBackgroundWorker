using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace backgroundworker.ScopedService
{
    public class ScopeSettingsService : IScopeSettingsService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ScopeSettingsService> _logger;

        public ScopeSettingsService(IConfiguration configuration, ILoggerFactory factory)
        {
            _configuration = configuration;
            _logger = factory.CreateLogger<ScopeSettingsService>();
        }

        public async Task WriteSetting(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var value = _configuration.GetValue<string>("Logging:LogLevel:Default");

                _logger.LogInformation(value);

                await Task.Delay(3_000, cancellationToken);
            }
        }
    }
}
