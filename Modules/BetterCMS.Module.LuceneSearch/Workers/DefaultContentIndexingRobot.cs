using System;
using BetterCms.Core.Dependencies;

namespace BetterCMS.Module.LuceneSearch.Workers
{
    public class DefaultContentIndexingRobot : WorkerBase
    {
        public DefaultContentIndexingRobot()
            : base(new TimeSpan(0, 0, 10, 0))
        {
        }

        protected override void DoWork()
        {
            using (var lifetimeScope = ContextScopeProvider.CreateChildContainer())
            {

            }      
        }
    }    
}
