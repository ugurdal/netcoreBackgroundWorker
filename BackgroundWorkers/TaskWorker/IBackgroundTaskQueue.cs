using System;
using System.Threading;
using System.Threading.Tasks;

namespace backgroundworker.TaskWorker
{
    public interface IBackgroundTaskQueue
    {
        void QueueTask(Func<CancellationToken, Task> workItem);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}