using System;
using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class Media : EquatableEntity<Media>
    {
        public virtual string Title { get; set; }
        
        public virtual MediaType Type { get; set; }
        
        public virtual MediaContentType ContentType { get; set; }

        public virtual MediaFolder Folder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Media" /> class.
        /// </summary>
        public Media()
        {
            ContentType = MediaContentType.File;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Title={1}, Type={2}", base.ToString(), Title, Type);
        }
    }
}