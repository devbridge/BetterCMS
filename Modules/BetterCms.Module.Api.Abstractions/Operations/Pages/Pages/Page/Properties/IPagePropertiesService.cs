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
        /// Replaces the page properties or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutPagePropertiesResponse</c> with a page properties id.</returns>
        PutPagePropertiesResponse Put(PutPagePropertiesRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostPagePropertiesResponse Post(PostPagePropertiesRequest request);

        /// <summary>
        /// Deletes the specified page properties.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeletePagePropertiesResponse</c> with success status.</returns>
        DeletePagePropertiesResponse Delete(DeletePagePropertiesRequest request);
    }
}