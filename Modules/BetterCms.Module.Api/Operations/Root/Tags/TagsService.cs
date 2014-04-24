using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    /// <summary>
    /// Tags service contract implementation for REST.
    /// </summary>
    public class TagsService : Service, ITagsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public TagsService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the tags list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetTagsResponse</c> with tags list.
        /// </returns>
        public GetTagsResponse Get(GetTagsRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Root.Models.Tag>()
                .Select(tag => new TagModel
                    {
                        Id = tag.Id,
                        Version = tag.Version,
                        CreatedBy = tag.CreatedByUser,
                        CreatedOn = tag.CreatedOn,
                        LastModifiedBy = tag.ModifiedByUser,
                        LastModifiedOn = tag.ModifiedOn,

                        Name = tag.Name
                    })
                .ToDataListResponse(request);

            return new GetTagsResponse
                       {
                           Data = listResponse
                       };
        }

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostTagsResponse</c> with a new tag id.
        /// </returns>
        public PostTagsResponse Post(PostTagsRequest request)
        {
            // TODO: implement.
            throw new System.NotImplementedException();
        }
    }
}