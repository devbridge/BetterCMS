using System;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [Route("/tags/{TagId}", Verbs = "GET")]
    [Route("/tags/by-name/{TagName}", Verbs = "GET")]
    public class GetTagRequest : RequestBase, IReturn<GetTagResponse>
    {
        /// <summary>
        /// Gets or sets the tag id.
        /// </summary>
        /// <value>
        /// The tag id.
        /// </value>
        public Guid? TagId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>
        /// The name of the tag.
        /// </value>
        public string TagName { get; set; }
    }
}