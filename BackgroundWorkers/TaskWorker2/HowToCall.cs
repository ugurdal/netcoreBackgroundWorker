using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace backgroundworker.TaskWorker2
{
    public class HowToCall
    {
        public class ExampleController : Controller
        {
            private readonly IBackgroundTaskQueue2 _backgroundTaskQueue;

            public ExampleController(IBackgroundTaskQueue2 backgroundTaskQueue)
            {
                _backgroundTaskQueue = backgroundTaskQueue ?? throw new ArgumentNullException(nameof(backgroundTaskQueue));
            }

            public IActionResult Index()
            {
                _backgroundTaskQueue.EnqueueTask(async (serviceScopeFactory, cancellationToken) =>
                {
                    // Get services
                    using var scope = serviceScopeFactory.CreateScope();
                    var myService = scope.ServiceProvider.GetRequiredService<IMyService>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<ExampleController>>();

                    try
                    {
                        // Do something expensive
                        await myService.DoSomethingAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Could not do something expensive");
                    }
                });

                return Ok();
            }
        }

        public interface IMyService
        {
            Task DoSomethingAsync(CancellationToken token);
        }
    }
}