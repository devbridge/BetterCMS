using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    [Route("/auhors/{AuthorId}", Verbs = "GET")]
    public class GetAuthorRequest : RequestBase, IReturn<GetAuthorResponse>
    {
        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        /// <value>
        /// The author id.
        /// </value>
        public System.Guid AuthorId { get; set; }
    }
}