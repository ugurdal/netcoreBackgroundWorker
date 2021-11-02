using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace backgroundworker.TaskWorker
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _workItems;
        private SemaphoreSlim _signal;

        public BackgroundTaskQueue()
        {
            _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
            _signal = new SemaphoreSlim(0);
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var item);
            return item;
        }

        public void QueueTask(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));

            _workItems.Enqueue(workItem);
            _signal.Release();
        }
    }
}