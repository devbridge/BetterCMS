using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Command.Extensions;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Videos.GetVideos
{
    public class GetVideosCommand : CommandBase, ICommand<MediaManagerViewModel, MediaManagerItemsViewModel>
    {
        private readonly IMediaVideoService mediaVideoService;

        public GetVideosCommand(IMediaVideoService mediaVideoService)
        {
            this.mediaVideoService = mediaVideoService;
        }

        public MediaManagerItemsViewModel Execute(MediaManagerViewModel request)
        {
            if (mediaVideoService == null)
            {
                return null;
            }

            request.SetDefaultSortingOptions("Title");
            var items = mediaVideoService.GetItems(request);
            return new MediaManagerItemsViewModel(items.Item1, request, items.Item2)
                {
                    Path = this.LoadPath(request, MediaType.Video)
                };
        }
    }
}