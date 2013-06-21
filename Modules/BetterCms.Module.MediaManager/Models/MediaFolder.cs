using System;
using System.Collections.Generic;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaFolder : Media
    {
        public virtual IList<Media> Medias { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFolder" /> class.
        /// </summary>
        public MediaFolder()
        {
            ContentType = MediaContentType.Folder;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("MediaFolder: {0}", base.ToString());
        }
    }
}