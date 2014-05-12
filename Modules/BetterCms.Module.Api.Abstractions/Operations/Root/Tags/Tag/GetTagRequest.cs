using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Request to get a tag.
    /// </summary>
    [Route("/tags/{TagId}", Verbs = "GET")]
    [Route("/tags/by-name/{TagName}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetTagRequest : IReturn<GetTagResponse>
    {
        /// <summary>
        /// Gets or sets the tag identifier.
        /// </summary>
        /// <value>
        /// The tag identifier.
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