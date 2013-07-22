using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [DataContract]
    public class GetTagModel
    {
        /// <summary>
        /// Gets or sets the tag id.
        /// </summary>
        /// <value>
        /// The tag id.
        /// </value>
        [DataMember]
        public Guid? TagId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>
        /// The name of the tag.
        /// </value>
        [DataMember]
        public string TagName { get; set; }
    }
}