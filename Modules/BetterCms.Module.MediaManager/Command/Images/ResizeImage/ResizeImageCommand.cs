using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.MediaManager.Services;

namespace BetterCms.Module.MediaManager.Command.Images
{
    public class ResizeImageCommand : CommandBase, ICommand<ResizeImageCommandRequest>
    {
        private readonly IMediaImageService mediaImageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeImageCommand" /> class.
        /// </summary>
        /// <param name="mediaImageService">The media image service.</param>
        public ResizeImageCommand(IMediaImageService mediaImageService)
        {
            this.mediaImageService = mediaImageService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void Execute(ResizeImageCommandRequest request)
        {
            mediaImageService.ResizeImage(
                request.Id,
                request.Version,
                request.Width,
                request.Height);
        }
    }
}
