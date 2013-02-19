using BetterCms.Core.DataServices;
using BetterCms.Core.Events;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultPageApiService : IPageApiService
    {
        public event PageCreatedEventArgs.PageCreatedEventHandler PageCreated;

        public void OnPageCreated(PageCreatedEventArgs args)
        {
            if (PageCreated != null)
            {
                PageCreated(this, args);
            }
        }
    }
}