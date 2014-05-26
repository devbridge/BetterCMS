using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    [DataContract]
    [Serializable]
    public class MediaTreeModel
    { 
        /// <summary>
        /// Gets or sets the tree of image medias.
        /// </summary>
        /// <value>
        /// The tree of image medias.
        /// </value>
        [DataMember]
        public IList<MediaItemModel> ImagesTree { get; set; }

        /// <summary>
        /// Gets or sets the tree of file medias.
        /// </summary>
        /// <value>
        /// The tree of file medias.
        /// </value>
        [DataMember]
        public IList<MediaItemModel> FilesTree { get; set; }
    }
}