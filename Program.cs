using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace backgroundworker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

            try
            {

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(log => log.ClearProviders())
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    // services.AddHostedService<Worker>();
                    // services.AddHostedService<LongRunner>();
                    // services.AddHostedService<Runner>();
                    services.AddHostedService<ScheduleService>();
                });
    }
}
