using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace backgroundworker.TaskWorker2
{
    public class BackgroundTaskQueue2 : IBackgroundTaskQueue2
    {
        private readonly ConcurrentQueue<Func<IServiceScopeFactory, CancellationToken, Task>> _items = new();

        // Holds the current count of tasks in the queue.
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void EnqueueTask(Func<IServiceScopeFactory, CancellationToken, Task> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _items.Enqueue(task);
            _signal.Release();
        }

        public async Task<Func<IServiceScopeFactory, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            // Wait for task to become available
            await _signal.WaitAsync(cancellationToken);

            _items.TryDequeue(out var task);
            return task;
        }
    }
}