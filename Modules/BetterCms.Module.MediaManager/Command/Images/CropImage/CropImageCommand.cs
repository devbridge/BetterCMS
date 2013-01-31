using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.MediaManager.ViewModels.Images;
using BetterCms.Module.MediaManager.Services;

namespace BetterCms.Module.MediaManager.Command.Images.CropImage
{
    public class CropImageCommand : CommandBase, ICommand<ImageViewModel>
    {
        private readonly IMediaImageService mediaImageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CropImageCommand" /> class.
        /// </summary>
        /// <param name="mediaImageService">The media image service.</param>
        public CropImageCommand(IMediaImageService mediaImageService)
        {
            this.mediaImageService = mediaImageService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void Execute(ImageViewModel request)
        {
            /*mediaImageService.CropImage(
                request.Id.ToGuidOrDefault(),
                request.Version.ToIntOrDefault(),
                request.CropCoordX1,
                request.CropCoordY1,
                request.CropCoordX2,
                request.CropCoordY2);*/
        }
    }
}
