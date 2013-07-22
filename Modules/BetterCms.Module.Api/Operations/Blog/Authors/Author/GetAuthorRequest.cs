using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    public class GetAuthorModel
    {
        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        /// <value>
        /// The author id.
        /// </value>
        public System.Guid AuthorId { get; set; }
    }

    [Route("/authors/{AuthorId}", Verbs = "GET")]
    public class GetAuthorRequest : RequestBase<GetAuthorModel>, IReturn<GetAuthorResponse>
    {
    }
}