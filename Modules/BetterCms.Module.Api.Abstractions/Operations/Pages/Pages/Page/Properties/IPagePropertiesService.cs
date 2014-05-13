namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page properties service contract for REST.
    /// </summary>
    public interface IPagePropertiesService
    {
        /// <summary>
        /// Gets the specified page properties.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetPagePropertiesResponse</c> with a page properties data.</returns>
        GetPagePropertiesResponse Get(GetPagePropertiesRequest request);

        /// <summary>
        /// Puts the page specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutPageResponse</c> with updated page id.</returns>
        PutPagePropertiesResponse Put(PutPagePropertiesRequest request);

        /// <summary>
        /// Posts the page specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostPagePropertiesRequest</c> with updated page id.</returns>
        PostPagePropertiesResponse Post(PostPagePropertiesRequest request);

        /// <summary>
        /// Deletes the page specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeletePageResponse</c> with success status.</returns>
        DeletePagePropertiesResponse Delete(DeletePagePropertiesRequest request);
    }
}