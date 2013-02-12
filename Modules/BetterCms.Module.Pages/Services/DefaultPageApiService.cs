using BetterCms.Api.Services;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultPageApiService : IPageApiService
    {
        public event Api.Events.PageCreatedEventArgs.PageCreatedEventHandler PageCreated;

        public void OnPageCreated(Api.Events.PageCreatedEventArgs args)
        {
            if (PageCreated != null)
            {
                PageCreated(this, args);
            }
        }
    }
}