using BetterCms.Core.Events;

namespace BetterCms.Core.DataServices
{
    public interface IPageApiService
    {
        // Events
        event PageCreatedEventArgs.PageCreatedEventHandler PageCreated;

        void OnPageCreated(PageCreatedEventArgs args);
    }
}
