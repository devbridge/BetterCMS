using System;
using System.Threading;
using System.Web.Hosting;

using Common.Logging;

namespace BetterCMS.Module.LuceneSearch.Workers
{
    public abstract class WorkerBase : IRegisteredObject, IWorker
    {
        protected static readonly ILog Log = LogManager.GetLogger(LuceneSearchConstants.LuceneSearchModuleLoggerNamespace);

        private readonly object lockObject = new object();        

        private readonly Thread workingThread;

        protected bool hostShuttingDown;

        private TimeSpan sleep;

        protected WorkerBase(TimeSpan sleep)
        {
            this.sleep = sleep;
            HostingEnvironment.RegisterObject(this);

            workingThread = new Thread(DoLongRunning);
        }

        protected abstract void DoWork();

        protected virtual void OnStop()
        {
        }

        public virtual void Stop(bool immediate)
        {
            hostShuttingDown = true;

            lock (lockObject)
            {
                OnStop();
                HostingEnvironment.UnregisterObject(this);
            }
        }

        public virtual void Start()
        {
            workingThread.Start();
        }
        
        private void DoLongRunning()
        {
            try
            {
                while (true)
                {
                    lock (lockObject)
                    {
                        if (hostShuttingDown)
                        {
                            return;
                        }

                        try
                        {
                            DoWork();
                        }
                        catch (Exception ex)
                        {
                            Log.WarnFormat("Lucene Search worker {0} failed. Retry in {1} seconds.", ex, GetType().Name, sleep.TotalSeconds);
                        }
                        
                        // Sleep before next run.
                        for (int i = 0; i < sleep.TotalSeconds; i++)
                        {
                            if (hostShuttingDown)
                            {
                                return;
                            }

                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.FatalFormat("Lucene Search worker {0} crashed.", ex, GetType().Name);
            }
        }
    }
}
