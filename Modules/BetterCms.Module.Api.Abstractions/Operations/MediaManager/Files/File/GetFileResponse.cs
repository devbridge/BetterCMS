using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    [DataContract]
    [System.Serializable]
    public class GetFileResponse : ResponseBase<FileModel>
    {
        /// <summary>
        /// Gets or sets the list of file tags.
        /// </summary>
        /// <value>
        /// The list of file tags.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<TagModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets the list of access rule models.
        /// </summary>
        /// <value>
        /// The list of access rule models.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<AccessRuleModel> AccessRules { get; set; }
    }
}