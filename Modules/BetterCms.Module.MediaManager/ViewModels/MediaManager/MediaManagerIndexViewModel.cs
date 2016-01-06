using BetterCms.Module.Root.Models;

using BetterModules.Core.Web.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    public class MediaManagerIndexViewModel : MediaViewModel
    {
        public UserMessages CustomFilesMessages { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, CustomFilesMessages: {1}", base.ToString(), CustomFilesMessages != null);
        }
    }
}