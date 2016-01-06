using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia
{
    /// <summary>
    /// Command for delete a media
    /// </summary>
    public class DeleteMediaCommand : CommandBase, ICommand<DeleteMediaCommandRequest, bool>
    {
        private readonly IMediaService mediaFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteMediaCommand" /> class.
        /// </summary>
        /// <param name="mediaFileService">The media file service.</param>
        public DeleteMediaCommand(IMediaService mediaFileService)
        {
            this.mediaFileService = mediaFileService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(DeleteMediaCommandRequest request)
        {
            return mediaFileService.DeleteMedia(request.Id, request.Version, true, Context.Principal);
        }
    }
}