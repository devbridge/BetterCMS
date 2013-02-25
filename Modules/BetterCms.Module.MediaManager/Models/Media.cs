using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class Media : EquatableEntity<Media>, IMedia
    {
        public virtual string Title { get; set; }
        
        public virtual MediaType Type { get; set; }

        public virtual MediaFolder Folder { get; set; }

        IMediaFolder IMedia.Folder
        {
            get { return Folder; }
        }
    }
}