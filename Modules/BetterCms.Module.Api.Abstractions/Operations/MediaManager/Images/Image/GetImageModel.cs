using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    [DataContract]
    [Serializable]
    public class GetImageModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include tags to response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags to response; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; } 
        
        [DataMember]
        public bool IncludeCategories { get; set; }
    }
}