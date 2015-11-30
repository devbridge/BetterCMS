// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkerBase.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
