using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueuedWorker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.StartAsync();

            var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();
            monitorLoop.StartMonitorLoop();

            await host.WaitForShutdownAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MonitorLoop>();
                    services.AddHostedService<QueuedHostedService>();
                    services.AddSingleton<IBackgroundTaskQueue>(ctx =>
                    {
                        if (!int.TryParse(hostContext.Configuration["QueueCapacity"], out var queueCapacity))
                            queueCapacity = 100;
                        return new BackgroundTaskQueue(queueCapacity);
                    });

                });
    }
}
