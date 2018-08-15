using System.Reactive.Concurrency;

namespace DineConnect.Common
{
    public interface IRxSchedulerService
    {
        IScheduler Immediate { get; }

        IScheduler CurrentThread { get; }

        IScheduler NewThread { get; }

        IScheduler TaskPool { get; }

        IScheduler ThreadPool { get; }
    }
}
