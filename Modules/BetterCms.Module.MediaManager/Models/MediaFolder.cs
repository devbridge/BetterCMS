using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaFolder : Media, IMediaFolder
    {
        public virtual MediaFolder ParentFolder { get; set; }

        public virtual IList<Media> Medias { get; set; }

        IMediaFolder IMediaFolder.ParentFolder
        {
            get { return ParentFolder; }
        }

        IEnumerable<IMedia> IMediaFolder.Medias
        {
            get { return Medias; }
        }
    }
}