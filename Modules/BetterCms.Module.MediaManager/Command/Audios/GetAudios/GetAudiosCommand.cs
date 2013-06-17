using BetterCms.Module.MediaManager.Command.MediaManager;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Command.Audios.GetAudios
{
    public class GetAudiosCommand : GetMediaItemsCommandBase<MediaFile>
    {
        /// <summary>
        /// Gets the type of the current media items.
        /// </summary>
        /// <value>
        /// The type of the current media items.
        /// </value>
        protected override MediaType MediaType
        {
            get { return MediaType.Audio; }
        }
    }
}