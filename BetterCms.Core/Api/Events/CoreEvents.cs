using BetterCms.Api;
using BetterCms.Core.Environment.Host;

namespace BetterCms.Core.Api.Events
{
    public class CoreEvents : EventsBase
    {
        /// <summary>
        /// Occurs when a CMS host starts.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICmsHost>> HostStart;

        /// <summary>
        /// Called when [CMS host start].
        /// </summary>
        /// <param name="cmsHost">The CMS host.</param>
        public void OnHostStart(ICmsHost cmsHost)
        {
            if (HostStart != null)
            {
                HostStart(new SingleItemEventArgs<ICmsHost>(cmsHost));
            }
        }
    }
}
