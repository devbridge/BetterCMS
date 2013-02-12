using BetterCms.Api.Events;

namespace BetterCms.Api.Services
{
    public interface IPageApiService
    {
        // Events
        event PageCreatedEventArgs.PageCreatedEventHandler PageCreated;

        void OnPageCreated(PageCreatedEventArgs args);
    }
}
