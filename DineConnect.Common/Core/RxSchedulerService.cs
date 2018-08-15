using System.Reactive.Concurrency;

namespace DineConnect.Common
{
    public class RxSchedulerService : IRxSchedulerService
    {
        public IScheduler Immediate
        {
            get { return Scheduler.Immediate; }
        }

        public IScheduler CurrentThread
        {
            get { return Scheduler.CurrentThread; }
        }

        public IScheduler NewThread
        {
            get { return NewThreadScheduler.Default; }
        }

        public IScheduler ThreadPool
        {
            get { return ThreadPoolScheduler.Instance; }
        }

        public IScheduler TaskPool
        {
            get { return TaskPoolScheduler.Default; }
        }
    }
}
