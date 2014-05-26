using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings
{
    [DataContract]
    [Serializable]
    public class SaveBlogPostsSettingsModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the default layout identifier.
        /// </summary>
        /// <value>
        /// The default layout identifier.
        /// </value>
        [DataMember]
        public Guid? DefaultLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the default master page identifier.
        /// </summary>
        /// <value>
        /// The default master page identifier.
        /// </value>
        [DataMember]
        public Guid? DefaultMasterPageId { get; set; }
    }
}