using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;

namespace BetterCms.Module.Api.Operations.Root.Tags
{

    /// <summary>
    /// Default tags service contract implementation for REST.
    /// </summary>
    [RoutePrefix("bcms-api")]
    public class TagsController : ApiController, ITagsService
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
        /// Initializes a new instance of the <see cref="TagsController" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="tagService">The tag service.</param>
        public TagsController(IRepository repository, ITagService tagService)
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
        [Route("tags")]
        public GetTagsResponse Get([ModelBinder(typeof(JsonModelBinder))]GetTagsRequest request)
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
        [Route("tags")]
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