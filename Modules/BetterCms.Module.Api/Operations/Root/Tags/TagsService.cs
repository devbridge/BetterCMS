using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Tags
{

    /// <summary>
    /// Default tags service contract implementation for REST.
    /// </summary>
    public class TagsService : Service, ITagsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The tag service.
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="tagService">The tag service.</param>
        public TagsService(IRepository repository, ITagService tagService)
        {
            this.repository = repository;
            this.tagService = tagService;
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
        public PostTagResponse Post(PostTagRequest request)
        {
            var result =
                tagService.Put(
                    new PutTagRequest
                        {
                            Data = request.Data,
                            User = request.User
                        });

            return new PostTagResponse { Data = result.Data };
        }
    }
}